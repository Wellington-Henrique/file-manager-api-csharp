using FluentValidation;
using FluentValidation.Results;

namespace FileUploadAPI.Entities
{
    public class FileUploadModel
    {
        public ValidationResult ValidationResult { get; set; }

        public FileUploadModel(IFormFile file)
        {
            File = file;
        }

        public IFormFile File { get; set; }

        public bool IsInvalid(AbstractValidator<FileUploadModel> validator)
        {
            ValidationResult = validator.Validate(this);
            return !ValidationResult.IsValid;
        }
    }

    public class FileUploadModelValidator : AbstractValidator<FileUploadModel>
    {
        public FileUploadModelValidator()
        {
            RuleFor(x => x.File)
                .NotEmpty().WithMessage("File is required.")
                .Must(file => file?.Length > 0).WithMessage("File is invalid.")
                .Must(file => file?.Length <= 1 * 1024 * 1024)
                .WithMessage("File size exceeds the limit of 1 MB.");
        }
    }
}
