﻿using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services;

public class AnswerService(SurveyDbContext context) : IAnswerService
{
    public async Task<Answer> CreateAnswerAsync(AnswerCreateDto answerDto)
    {
        // Получаем вопрос
        var question = await context.Questions.Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == answerDto.QuestionId);
        
        if (question == null)
        {
            throw new ArgumentException("Invalid question ID");
        }

        // Создаем объект для ответа
        var answer = new Answer(Guid.NewGuid(), answerDto.QuestionId, answerDto.AnswerText, answerDto.OptionId, answerDto.UserId);

        // Логика в зависимости от типа вопроса
        if (question.Type == QuestionType.TextAnswer)
        {
            // Для текстового ответа
            if (string.IsNullOrWhiteSpace(answerDto.AnswerText))
            {
                throw new ArgumentException("Text answer cannot be empty.");
            }

            answer = answer with { AnswerText = answerDto.AnswerText };
        }
        else if (question.Type == QuestionType.SingleChoice || question.Type == QuestionType.MultipleChoice)
        {
            // Для вопросов с выбором (MultipleChoice или SingleChoice)
            if (answerDto.OptionIds != null && answerDto.OptionIds.Any())
            {
                // Для MultipleChoice или SingleChoice проверяем, что опции существуют
                var selectedOptions = new List<Option>();
                foreach (var optionId in answerDto.OptionIds)
                {
                    var option = question.Options.FirstOrDefault(o => o.Id == optionId);
                    if (option != null)
                    {
                        selectedOptions.Add(option);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid option ID");
                    }
                }

                // Для SingleChoice, мы разрешаем только один выбор
                if (question.Type == QuestionType.SingleChoice && selectedOptions.Count > 1)
                {
                    throw new ArgumentException("Single choice question can only have one selected option.");
                }

                // Добавляем выбранные опции в ответ
                answer.SelectedOptions = selectedOptions;
            }
            else
            {
                throw new ArgumentException("No options selected for a multiple choice or single choice question.");
            }
        }

        // Проверка существования пользователя
        var userExists = await context.Users.AnyAsync(u => u.Id == answerDto.UserId);
        if (userExists)
        {
            answer.User = await context.Users.FirstOrDefaultAsync(a => a.Id == answerDto.UserId);
        }
        else
        {
            throw new ArgumentException("Invalid User ID");
        }

        // Сохраняем ответ
        context.Answers.Add(answer);
        await context.SaveChangesAsync();

        return answer;
    }

    public async Task<Answer> GetAnswerByIdAsync(Guid id)
    {
        return await context.Answers
            .Include(a => a.Question)
            .Include(a => a.Option)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<OptionWithAnswerCountDto>> GetOptionsWithAnswerCountAsync(Guid surveyId)
    {
        var survey = await context.Surveys
            .Include(s => s.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(s => s.Id == surveyId);

        if (survey == null)
            throw new ArgumentException($"Survey with ID {surveyId} not found.");

        var questionsId = survey.Questions.Select(q => q.Id).ToList();

        var optionsWithCounts = await context.Answers
            .Where(a => questionsId.Contains(a.QuestionId))
            .GroupBy(a => a.OptionId)
            .Select(g => new
            {
                OptionId = g.Key,
                AnswerCount = g.Count()
            })
            .ToListAsync();

        var result = survey.Questions
            .SelectMany(q => q.Options)
            .Select(option => new OptionWithAnswerCountDto
            {
                OptionId = option.Id,
                Text = option.Text,
                AnswerCount = optionsWithCounts.FirstOrDefault(o => o.OptionId == option.Id)?.AnswerCount ?? 0
            })
            .ToList();

        return result;
    }
}