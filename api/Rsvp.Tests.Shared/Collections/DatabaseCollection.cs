namespace Rsvp.Tests.Shared.Collections;


using Rsvp.Tests.Shared.Fixtures.Database;

using Xunit;

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }


