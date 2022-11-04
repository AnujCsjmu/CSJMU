using CoreLayout.Models;
using CoreLayout.Models.Exam;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.ExamCategory
{
    public interface IExamCategoryRepository
    {
       Task<List<ExamCategoryModel>> GetExamCategoryAsync();
    }
}
