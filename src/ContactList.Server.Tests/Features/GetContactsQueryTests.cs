using ContactList.Contracts;
using ContactList.Server.Model;

namespace ContactList.Server.Tests.Features;

class GetContactsQueryTests
{
    public async Task ShouldGetAllContactsSortedByName()
    {
        var benEmail = SampleEmail();
        var benPhoneNumber = SamplePhoneNumber();
        var ben = new AddContactCommand
        {
            Email = benEmail,
            Name = "Ben",
            PhoneNumber = benPhoneNumber
        };

        var cathyEmail = SampleEmail();
        var cathyPhoneNumber = SamplePhoneNumber();
        var cathy = new AddContactCommand
        {
            Email = cathyEmail,
            Name = "Cathy",
            PhoneNumber = cathyPhoneNumber
        };

        var abeEmail = SampleEmail();
        var abePhoneNumber = SamplePhoneNumber();
        var abe = new AddContactCommand
        {
            Email = abeEmail,
            Name = "Abe",
            PhoneNumber = abePhoneNumber
        };

        var benId = (await SendAsync(ben)).ContactId;
        var cathyId = (await SendAsync(cathy)).ContactId;
        var abeId = (await SendAsync(abe)).ContactId;

        var expectedIds = new[] { benId, cathyId, abeId };

        var result = await SendAsync(new GetContactsQuery());

        result.Length.ShouldBe(await CountAsync<Contact>());

        result
            .Where(x => expectedIds.Contains(x.Id))
            .ShouldMatch(
                new ContactViewModel("Abe", abeEmail, abePhoneNumber) { Id = abeId },
                new ContactViewModel("Ben", benEmail, benPhoneNumber) { Id = benId },
                new ContactViewModel("Cathy", cathyEmail, cathyPhoneNumber) { Id = cathyId }
            );
    }
}
