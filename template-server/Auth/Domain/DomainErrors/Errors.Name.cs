namespace Domain.DomainErrors;

public static partial class Errors
{
    public static class Name
    {
        public static Error IsRequired => new Error("name.is.required");

        public static Error MustBeAtLeastTwoCharacters =>
            new Error("name.must.be.at.least.two.characters.long");

        public static Error MustContainOnlyLetters => new Error("name.must.contain.only.letters");
    }
}
