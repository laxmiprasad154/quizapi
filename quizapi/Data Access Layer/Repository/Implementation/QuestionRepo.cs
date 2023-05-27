using quizapi.Data_Access_Layer.context;
using quizapi.Data_Access_Layer.Entities;
﻿using Microsoft.EntityFrameworkCore;
using quizapi.Data_Access_Layer.Repository.Interface;

namespace quizapi.Data_Access_Layer.Repository.Implementation
{

    public class QuestionRepo : IQuestionListingRep
    {
        private readonly Quizdbcontext dbContext;

        public QuestionRepo(Quizdbcontext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Question> CreateAsync(Question question)
        {
            await dbContext.Questions.AddAsync(question);
            await dbContext.SaveChangesAsync();
            return question;
        }

       

        async Task<List<Question>> IQuestionListingRep.GetAllAsync()
        {
            return await dbContext.Questions.ToListAsync();
        }

        async Task<Question> IQuestionListingRep.GetByIdAsync(int id)
        {
            return await dbContext.Questions.FirstOrDefaultAsync(x => x.QnId == id);
        }

        public async Task<Question> UpdateAsync(int id, Question Question)
        {
            var existingQuestion = await dbContext.Questions.FirstOrDefaultAsync(x => x.QnId == id);
            if (existingQuestion == null)
            {
                return null;
            }

            existingQuestion.QnId = Question.QnId;
            existingQuestion.QnInWords = Question.QnInWords;
            existingQuestion.Option1= Question.Option1;
            existingQuestion.Option2 = Question.Option2;
            existingQuestion.Option3 = Question.Option3;
            existingQuestion.Option4 = Question.Option4;
            existingQuestion.Answer=Question.Answer;


            await dbContext.SaveChangesAsync();
            return existingQuestion;
        }

        async Task<Question> IQuestionListingRep.DeleteAsync(int id)
        {
            var existingQuestion = await dbContext.Questions.FirstOrDefaultAsync(x => x.QnId == id);
            if (existingQuestion == null)


            {
                return null;
            }
            dbContext.Questions.Remove(existingQuestion);

            await dbContext.SaveChangesAsync();
            return existingQuestion;
        }
    }
}
﻿