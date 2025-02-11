using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Entities.Concrete;
using FluentValidation;

namespace BookStoreApp.Business.ValidationRules.FluentValidation
{
    public class ReviewValidator:AbstractValidator<BookReview>
    {
        public ReviewValidator()
        {
            RuleFor(r => r.UserId).Must(BeCreatedByUser);
        }
        private bool BeCreatedByUser(BookReview review, Guid userId)
        {
            return review.UserId == userId;
        }
    }
}
