using Amazon.DynamoDBv2.DataModel;
using BeatGrid.Contracts.Common;
using BeatGrid.Data.Entities;
using BeatGrid.Data.Repositories;
using System;

namespace BeatGrid.ConsoleApp.Services
{
    public interface ITestUtil
    {
        void Test();
    }

    public class TestUtil : ITestUtil
    {
        private readonly IDynamoDBContext _context;
        private readonly IBeatRepository _beatRepository;

        public TestUtil(IBeatRepository beatRepository, IDynamoDBContext context)
        {
            _context = context;
            _beatRepository = beatRepository;
        }

        public void Test()
        {
            var testBeat = GenerateTestBeat();

            _context.SaveAsync(testBeat).Wait();

            var savedBeat = _context.LoadAsync(testBeat).Result;
        }

        private BeatEntity GenerateTestBeat()
        {
            return new BeatEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test beat 1",
                DivisionLevel = 16,
                Tempo = 90,
                TimeSignature = new TimeSignature
                {
                    NotesPerMeasure = 4,
                    NoteType = 16
                },
            };
        }
    }
}
