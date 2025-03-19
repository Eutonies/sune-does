using Booktex.Domain.Book.Model;

namespace SuneDoes.UI.Pages.Space;

public record SpaceFragment(
    SpaceFragmentSpecification Specification,
    IReadOnlyCollection<BookChapterContent> Content
    );
