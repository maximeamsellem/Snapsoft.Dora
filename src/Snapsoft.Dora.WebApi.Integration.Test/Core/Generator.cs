using Bogus;

namespace Snapsoft.Dora.WebApi.Integration.Test.Core;

internal static class Generator
{
    internal static string RandomString(int length = 10)
    {
        return new Faker().Random.AlphaNumeric(length);
    }
}
