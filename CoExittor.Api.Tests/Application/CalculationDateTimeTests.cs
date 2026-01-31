using CoExittor.Api.Application.Services;
using CoExittor.Common.Models;

namespace CoExittor.Api.Tests.Application
{
    public class CalculationDateTimeTests
    {
        [Fact]
        public void Calculate_AgreedDate_NoOverlap_ReturnsEmptyList()
        {
            // Arrange
            List<Voting> expected = [];

            Event @event = new()
            {
                Participants =
                [
                    new() {
                        IsHost = true,
                        IsAgreedWithDefault = true,
                        Votings =
                        [
                            new() {
                                StartDate = new DateTime(2024, 7, 1, hour: 18, minute: 00, second: 00),
                                EndDate = new DateTime(2024, 7, 1, hour: 23, minute: 00, second: 00)
                            }
                        ]
                    },
                    new() {
                        IsHost = false,
                        IsAgreedWithDefault = false,
                        Votings =
                        [
                            new Voting
                            {
                                StartDate = new DateTime(2024, 7, 6),
                                EndDate = new DateTime(2024, 7, 10)
                            }
                        ]
                    }
                ]
            };
            // Act
            List<Voting> actual = EventService.CalculateAgreedDate(@event);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Calculate_AgreedDate_WithOverlap_ReturnsOverlapping()
        {
            // Arrange
            List<Voting> expected = 
            [
                new()
                {
                    StartDate = new DateTime(2024, 7, 5),
                    EndDate = new DateTime(2024, 7, 7)
                }
            ];
            Event @event = new()
            {
                Participants =
                [
                    new() {
                        IsHost = true,
                        IsAgreedWithDefault = true,
                        Votings =
                        [
                            new() {
                                StartDate = new DateTime(2024, 7, 1),
                                EndDate = new DateTime(2024, 7, 10)
                            }
                        ]
                    },
                    new() {
                        IsHost = false,
                        IsAgreedWithDefault = false,
                        Votings =
                        [
                            new Voting
                            {
                                StartDate = new DateTime(2024, 7, 5),
                                EndDate = new DateTime(2024, 7, 7)
                            }
                        ]
                    }
                ]
            };
            // Act
            List<Voting> actual = EventService.CalculateAgreedDate(@event);
            // Assert
            Assert.Single(actual);
            Assert.Equal(expected[0].StartDate, actual[0].StartDate);
            Assert.Equal(expected[0].EndDate, actual[0].EndDate);
        }

        [Fact]
        public void Calculate_AgreedDateWithTime_WithOverlap_ReturnsOverlapping()
        {
            // Arrange
            List<Voting> expected =
            [
                new()
                {
                    StartDate = new DateTime(2024, 7, 1, hour: 20, minute: 00, second: 00),
                    EndDate = new DateTime(2024, 7, 1, hour: 22, minute: 00, second: 00)
                }
            ];
            Event @event = new()
            {
                Participants =
                [
                    new() {
                        IsHost = true,
                        IsAgreedWithDefault = true,
                        Votings =
                        [
                            new() {
                                StartDate = new DateTime(2024, 7, 1, hour: 18, minute: 00, second: 00),
                                EndDate = new DateTime(2024, 7, 1, hour: 23, minute: 00, second: 00)
                            }
                        ]
                    },
                    new() {
                        IsHost = false,
                        IsAgreedWithDefault = false,
                        Votings =
                        [
                            new Voting
                            {
                                StartDate = new DateTime(2024, 7, 1, hour: 20, minute: 00, second: 00),
                                EndDate = new DateTime(2024, 7, 1, hour: 22, minute: 00, second: 00)
                            }
                        ]
                    }
                ]
            };
            // Act
            List<Voting> actual = EventService.CalculateAgreedDate(@event);
            // Assert
            Assert.Single(actual);
            Assert.Equal(expected[0].StartDate, actual[0].StartDate);
            Assert.Equal(expected[0].EndDate, actual[0].EndDate);
        }
    }
}
