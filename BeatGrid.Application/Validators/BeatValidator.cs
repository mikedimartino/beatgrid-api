using BeatGrid.Contracts.Common;
using FluentValidation;
using System.Collections.Generic;
  
namespace BeatGrid.Application.Validators
{
    public class BeatValidator : AbstractValidator<Beat>
    {
        private readonly HashSet<int> _validNoteTypes = new HashSet<int> { 2, 4, 8, 16, 32, 64 };
        private readonly HashSet<int> _validDivisionLevels = new HashSet<int> { 2, 4, 8, 16, 32, 64 };

        public BeatValidator()
        {
            RuleFor(b => b.Id).NotEmpty();

            RuleFor(b => b.Name).NotEmpty();

            RuleFor(b => b.Tempo).GreaterThan(0);

            RuleFor(b => b.DivisionLevel).Must(d => _validDivisionLevels.Contains(d))
                .WithMessage($"Division level must be one of following: {string.Join(", ", _validDivisionLevels)}.");

            RuleFor(b => b.TimeSignature).NotNull();
            When(b => b.TimeSignature != null, () =>
            {
                RuleFor(b => b.TimeSignature.NotesPerMeasure).GreaterThan(0);
                RuleFor(b => b.TimeSignature.NoteType).Must(n => _validNoteTypes.Contains(n))
                    .WithMessage($"TimeSignature.NoteType must be one of following: {string.Join(", ", _validNoteTypes)}.");
            });

            RuleFor(b => b.Measures).NotEmpty();
            // TODO: Further validation on activeSquares (cols / rows)

            RuleFor(b => b.Rows).NotEmpty();
            // TODO: Further validation on rows
        }
    }
}
