using CoExittor.Api.Application.Services;
using CoExittor.Common.Models;

namespace CoExittor.Api.Tests.Application
{
    public class CalculationDateTimeTests
    {
        [Fact]
        public void AgreedDate_NoOverlap_ReturnsEmptyList()
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
        public void AgreedDate_WithOverlap_ReturnsOverlapping()
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
        public void AgreedDateWithTime_WithOverlap_ReturnsOverlapping()
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

        [Fact]
        public void AgreedDate_WithOverlap_MultipleParticipants()
        {
            // Arrange
            List<Voting> expected =
            [
                new()
                {
                    StartDate = new DateTime(2026, 1, 2, hour: 12, minute: 30, second: 00),
                    EndDate = new DateTime(2026, 1, 2, hour: 12, minute: 45, second: 00)
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
                                StartDate = new DateTime(2026, 1, 2, hour: 11, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 1, 2, hour: 14, minute: 00, second: 00)
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
                                StartDate = new DateTime(2026, 1, 2, hour: 12, minute: 30, second: 00),
                                EndDate = new DateTime(2026, 1, 2, hour: 13, minute: 00, second: 00)
                            },
                            new Voting
                            {
                                StartDate = new DateTime(2026, 1, 2, hour: 16, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 1, 2, hour: 17, minute: 00, second: 00)
                            },
                        ]
                    },
                    new() {
                        IsHost = false,
                        IsAgreedWithDefault = false,
                        Votings =
                        [
                            new Voting
                            {
                                StartDate = new DateTime(2026, 1, 2, hour: 12, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 1, 2, hour: 12, minute: 45, second: 00)
                            },
                            new Voting
                            {
                                StartDate = new DateTime(2026, 1, 2, hour: 16, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 1, 2, hour: 17, minute: 00, second: 00)
                            },
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
        public void AgreedDate_WithOverlap_DefaultExampleCase()
        {
            // Arrange
            List<Voting> expected =
            [
                new()
                {
                    StartDate = new DateTime(2026, 4, 10, hour: 21, minute: 00, second: 00),
                    EndDate = new DateTime(2026, 4, 10, hour: 22, minute: 00, second: 00)
                }
            ];
            Event @event = new()
            {
                Participants =
                [
                    new() {
                        Name = "Петя",
                        IsHost = true,
                        IsAgreedWithDefault = true,
                        Votings =
                        [
                            new() {
                                StartDate = new DateTime(2026, 4, 10, hour: 18, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 4, 10, hour: 22, minute: 00, second: 00)
                            },
                            new() {
                                StartDate = new DateTime(2026, 4, 11, hour: 18, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 4, 11, hour: 22, minute: 00, second: 00)
                            },
                            new() {
                                StartDate = new DateTime(2026, 4, 17, hour: 18, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 4, 17, hour: 22, minute: 00, second: 00)
                            },
                            new() {
                                StartDate = new DateTime(2026, 4, 18, hour: 18, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 4, 18, hour: 22, minute: 00, second: 00)
                            }
                        ]
                    },
                    new() {
                        Name = "Вася",
                        IsHost = false,
                        IsAgreedWithDefault = false,
                        Votings =
                        [
                            new() {
                                StartDate = new DateTime(2026, 4, 9, hour: 20, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 4, 9, hour: 23, minute: 00, second: 00)
                            },
                            new() {
                                StartDate = new DateTime(2026, 4, 10, hour: 19, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 4, 10, hour: 22, minute: 00, second: 00)
                            },
                            new() {
                                StartDate = new DateTime(2026, 4, 16, hour: 21, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 4, 16, hour: 23, minute: 00, second: 00)
                            }
                        ]
                    },
                    new() {
                        Name = "Серега",
                        IsHost = false,
                        IsAgreedWithDefault = false,
                        Votings =
                        [
                            new() {
                                StartDate = new DateTime(2026, 4, 8, hour: 21, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 4, 8, hour: 23, minute: 00, second: 00)
                            },
                            new() {
                                StartDate = new DateTime(2026, 4, 9, hour: 19, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 4, 9, hour: 22, minute: 00, second: 00)
                            },
                            new() {
                                StartDate = new DateTime(2026, 4, 10, hour: 21, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 4, 10, hour: 23, minute: 00, second: 00)
                            },
                            new() {
                                StartDate = new DateTime(2026, 4, 11, hour: 19, minute: 00, second: 00),
                                EndDate = new DateTime(2026, 4, 11, hour: 23, minute: 00, second: 00)
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
