using FluentAssertions;
using NoteTestApi;
using ProjectTestingNote.DTO;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace TestForNotesProject
{
    public class NoteApiIntegrationTests : IClassFixture<TestAppFactory>
    {

        private readonly HttpClient _httpClient;
        public NoteApiIntegrationTests(TestAppFactory factory)
        {
            _httpClient = factory.CreateClient();
        }
        [Fact]

        public async Task Create_Returns_201()
        {
            var request = new NoteCreateDto("Test Note", "This is a test note.");
            var postResponse = await _httpClient.PostAsJsonAsync("/api/note/create", request);
            postResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            var createdNote = await postResponse.Content.ReadFromJsonAsync<NoteDTO>();
            createdNote.Should().NotBeNull();
            createdNote!.Id.Should().NotBeEmpty();
            createdNote.Title.Should().Be("Test Note");
            createdNote.Content.Should().Be("This is a test note.");

            var getResponce = await _httpClient.GetAsync($"/api/note/{createdNote.Id}");
            getResponce.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var fetchedNote = await getResponce.Content.ReadFromJsonAsync<NoteDTO>();
            fetchedNote.Should().NotBeNull();
            fetchedNote.Should().BeEquivalentTo(createdNote, options => options.Excluding(x => x.CreatedAt));
            fetchedNote!.Id.Should().NotBeEmpty();
            fetchedNote.Title.Should().Be("Test Note");
            fetchedNote.Content.Should().Be("This is a test note.");

        }
        [Fact]
        public async Task Create_Returns_400_For_Invalid_Data()
        {
            var request = new NoteCreateDto("", "This is a test mode with empty title.");

            var postResponce = await _httpClient.PostAsJsonAsync("/api/note/create-Note", request);

            postResponce.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            request = new NoteCreateDto("Test Note with Empty Content", "");
            postResponce  = await _httpClient.PostAsJsonAsync("/api/note/create-Note", request);

            postResponce.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
