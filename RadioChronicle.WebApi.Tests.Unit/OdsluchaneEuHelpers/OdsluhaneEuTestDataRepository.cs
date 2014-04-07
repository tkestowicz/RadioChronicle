using System;
using System.Collections.Generic;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Tests.Unit.OdsluchaneEuHelpers
{
    internal static class OdsluhaneEuTestDataRepository
    {
        public static IEnumerable<Track> ExpectedMostPopularTracksOnRMFFMRadioStationInMay2013
        {
            get
            {
                return new List<Track>()
                {
                    new Track()
                    {
                        Name = "One Direction - One Way Or Another",
                        RelativeUrlToTrackDetails = "/utwor/141032/one_direction_-_one_way_or_another",
                        TimesPlayed = 122,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Pink / Nate Ruess - Just Give Me A Reason",
                        RelativeUrlToTrackDetails = "/utwor/137317/pink_nate_ruess_-_just_give_me_a_reason",
                        TimesPlayed = 116,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Rihanna - Stay",
                        RelativeUrlToTrackDetails = "/utwor/123861/rihanna_-_stay",
                        TimesPlayed = 112,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Dido - No Freedom",
                        RelativeUrlToTrackDetails = "/utwor/134632/dido_-_no_freedom",
                        TimesPlayed = 110,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Lana Del Rey - Dark Paradise",
                        RelativeUrlToTrackDetails = "/utwor/115431/lana_del_rey_-_dark_paradise",
                        TimesPlayed = 105,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Justin Timberlake - Mirrors",
                        RelativeUrlToTrackDetails = "/utwor/141039/justin_timberlake_-_mirrors",
                        TimesPlayed = 101,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Lemon - Napraw",
                        RelativeUrlToTrackDetails = "/utwor/124264/lemon_-_napraw",
                        TimesPlayed = 96,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Passenger - Let Her Go",
                        RelativeUrlToTrackDetails = "/utwor/123001/passenger_-_let_her_go",
                        TimesPlayed = 95,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Bastille - Pompeii",
                        RelativeUrlToTrackDetails = "/utwor/136911/bastille_-_pompeii",
                        TimesPlayed = 94,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Honorata Skarbek Honey - Nie Powiem Jak",
                        RelativeUrlToTrackDetails = "/utwor/145876/honorata_skarbek_honey_-_nie_powiem_jak",
                        TimesPlayed = 93,
                        TrackHistory = new List<TrackHistory>()
                    }
                };
            }
        }

        public static IEnumerable<RadioStationGroup> ExpectedRadioStationGroups
        {
            get
            {
                return new List<RadioStationGroup>()
                {
                    new RadioStationGroup()
                    {
                        GroupName = "Eurozet",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 1,
                                Name = "Radio ZET"
                            },
                            new RadioStation()
                            {
                                Id = 40,
                                Name = "Chilli ZET"
                            },
                            new RadioStation()
                            {
                                Id = 7,
                                Name = "Planeta FM (Warszawa)"
                            },
                            new RadioStation()
                            {
                                Id = 5,
                                Name = "Antyradio"
                            },
                            new RadioStation()
                            {
                                Id = 8,
                                Name = "Radio Plus"
                            }
                        }
                    },
                    new RadioStationGroup()
                    {
                        GroupName = "RMF",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 2,
                                Name = "RMF FM"
                            },
                            new RadioStation()
                            {
                                Id = 4,
                                Name = "RMF MAXXX"
                            },
                            new RadioStation()
                            {
                                Id = 6,
                                Name = "RMF Classic"
                            },
                            new RadioStation()
                            {
                                Id = 11,
                                Name = "RMF Dance"
                            }
                        }
                    },
                    new RadioStationGroup()
                    {
                        GroupName = "ESKA",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 3,
                                Name = "Radio Eska"
                            },
                            new RadioStation()
                            {
                                Id = 10,
                                Name = "Eska Rock"
                            }
                        }
                    },
                    new RadioStationGroup()
                    {
                        GroupName = "Grupa Radiowa Agory",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 9,
                                Name = "Złote Przeboje"
                            },
                            new RadioStation()
                            {
                                Id = 13,
                                Name = "Roxy FM"
                            },
                            new RadioStation()
                            {
                                Id = 17,
                                Name = "Blue FM (Poznań)"
                            }
                        }
                    },
                    new RadioStationGroup()
                    {
                        GroupName = "Polskie Radio",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 46,
                                Name = "Jedynka"
                            },
                            new RadioStation()
                            {
                                Id = 47,
                                Name = "Dwójka"
                            },
                            new RadioStation()
                            {
                                Id = 48,
                                Name = "Trójka"
                            },
                            new RadioStation()
                            {
                                Id = 49,
                                Name = "Czwórka"
                            }
                        }
                    },
                    new RadioStationGroup()
                    {
                        GroupName = "Radio FaMa",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 19,
                                Name = "Radio FaMa (Kielce)"
                            },
                            new RadioStation()
                            {
                                Id = 18,
                                Name = "Radio FaMa (Tomaszów Maz.)"
                            }
                        }
                    },
                    new RadioStationGroup()
                    {
                        GroupName = "Pozostałe",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 23,
                                Name = "Muzyczne Radio"
                            },
                            new RadioStation()
                            {
                                Id = 16,
                                Name = "Radio Express"
                            },
                            new RadioStation()
                            {
                                Id = 14,
                                Name = "Radio PiN"
                            },
                            new RadioStation()
                            {
                                Id = 20,
                                Name = "Radio GRA"
                            },
                            new RadioStation()
                            {
                                Id = 15,
                                Name = "MC Radio"
                            },
                            new RadioStation()
                            {
                                Id = 12,
                                Name = "Polskie Radio Londyn"
                            },
                            new RadioStation()
                            {
                                Id = 22,
                                Name = "Radio eM"
                            },
                            new RadioStation()
                            {
                                Id = 21,
                                Name = "Radio RSC"
                            },
                            new RadioStation()
                            {
                                Id = 24,
                                Name = "Radio Elka"
                            },
                            new RadioStation()
                            {
                                Id = 25,
                                Name = "Radio Kolor"
                            },
                            new RadioStation()
                            {
                                Id = 26,
                                Name = "Radio Kaszebe"
                            },
                            new RadioStation()
                            {
                                Id = 27,
                                Name = "Radio Merkury"
                            },
                            new RadioStation()
                            {
                                Id = 28,
                                Name = "Radio Park"
                            },
                            new RadioStation()
                            {
                                Id = 29,
                                Name = "Radio Centrum (Kalisz)"
                            },
                            new RadioStation()
                            {
                                Id = 30,
                                Name = "VAX FM"
                            },
                            new RadioStation()
                            {
                                Id = 31,
                                Name = "Radio WAWA"
                            },
                            new RadioStation()
                            {
                                Id = 32,
                                Name = "Radio 90"
                            },
                            new RadioStation()
                            {
                                Id = 35,
                                Name = "Radio Vanessa"
                            },
                            new RadioStation()
                            {
                                Id = 41,
                                Name = "Radio Leliwa"
                            },
                            new RadioStation()
                            {
                                Id = 42,
                                Name = "Radio Freee"
                            },
                            new RadioStation()
                            {
                                Id = 43,
                                Name = "Radio Traffic"
                            },
                            new RadioStation()
                            {
                                Id = 44,
                                Name = "Radio Bielsko"
                            },
                            new RadioStation()
                            {
                                Id = 45,
                                Name = "Radio RAM"
                            },
                        }
                    }
                };
            }
        }

        public static IDictionary<DateTime, IEnumerable<Track>> ExpectedNewestTracksOnRMFFm
        {
            get
            {
                return new Dictionary<DateTime, IEnumerable<Track>>
                {
                    {
                        new DateTime(2013, 5, 31), new List<Track>()
                        {
                            new Track()
                            {
                                PlayedFirstTime = new DateTime(2013, 5, 31, 19, 34, 0),
                                Name = "Video / Jan Borysewicz - Kryzysowy",
                                RelativeUrlToTrackDetails = "/utwor/157779/video_jan_borysewicz_-_kryzysowy",
                                TimesPlayed = 0,
                                TrackHistory = new List<TrackHistory>()
                            }
                        }
                    },
                    {
                        new DateTime(2013, 5, 28), new List<Track>()
                        {
                            new Track()
                            {
                                PlayedFirstTime = new DateTime(2013, 5, 28, 3, 49, 0),
                                Name = "Celine Dion - Tous Les Secrets",
                                RelativeUrlToTrackDetails = "/utwor/118748/celine_dion_-_tous_les_secrets",
                                TimesPlayed = 0,
                                TrackHistory = new List<TrackHistory>()
                            }
                        }
                    },
                    {
                        new DateTime(2013, 5, 27), new List<Track>()
                        {
                            new Track()
                            {
                                PlayedFirstTime = new DateTime(2013, 5, 27, 20, 19, 0),
                                Name = "Dawid Podsiadło - Trójkąty I Kwadraty",
                                RelativeUrlToTrackDetails = "/utwor/151772/dawid_podsiadlo_-_trojkaty_i_kwadraty",
                                TimesPlayed = 0,
                                TrackHistory = new List<TrackHistory>()
                            }
                        }
                    },
                    {
                        new DateTime(2013, 5, 25), new List<Track>()
                        {
                            new Track()
                            {
                                PlayedFirstTime = new DateTime(2013, 5, 25, 22, 17, 0),
                                Name = "Counting Crows - Big Yellow Taxi",
                                RelativeUrlToTrackDetails = "/utwor/64339/counting_crows_-_big_yellow_taxi",
                                TimesPlayed = 0,
                                TrackHistory = new List<TrackHistory>()
                            },
                            new Track()
                            {
                                PlayedFirstTime = new DateTime(2013, 5, 25, 4, 27, 0),
                                Name = "Marta Podulka - Nieodkryty Ląd",
                                RelativeUrlToTrackDetails = "/utwor/156172/marta_podulka_-_nieodkryty_lad",
                                TimesPlayed = 0,
                                TrackHistory = new List<TrackHistory>()
                            },
                            new Track()
                            {
                                PlayedFirstTime = new DateTime(2013, 5, 25, 1, 25, 0),
                                Name = "Red Lips - To Co Nam Było",
                                RelativeUrlToTrackDetails = "/utwor/156496/red_lips_-_to_co_nam_bylo",
                                TimesPlayed = 0,
                                TrackHistory = new List<TrackHistory>()
                            },
                            new Track()
                            {
                                PlayedFirstTime = new DateTime(2013, 5, 25, 1, 24, 0),
                                Name = "Blondie - The Tide Is High",
                                RelativeUrlToTrackDetails = "/utwor/22211/blondie_-_the_tide_is_high",
                                TimesPlayed = 0,
                                TrackHistory = new List<TrackHistory>()
                            }
                        }
                    },
                    {
                        new DateTime(2013, 5, 21), new List<Track>()
                        {
                            new Track()
                            {
                                PlayedFirstTime = new DateTime(2013, 5, 21, 20, 20, 0),
                                Name = "Lana Del Rey - Young And Beautiful",
                                RelativeUrlToTrackDetails = "/utwor/151740/lana_del_rey_-_young_and_beautiful",
                                TimesPlayed = 0,
                                TrackHistory = new List<TrackHistory>()
                            },
                        }
                    },
                    {
                        new DateTime(2013, 5, 17), new List<Track>()
                        {
                            new Track()
                            {
                                PlayedFirstTime = new DateTime(2013, 5, 17, 20, 22, 0),
                                Name = "Naughty Boy / Sam Smith - La La La",
                                RelativeUrlToTrackDetails = "/utwor/146755/naughty_boy_sam_smith_-_la_la_la",
                                TimesPlayed = 0,
                                TrackHistory = new List<TrackHistory>()
                            },
                        }
                    },
                    {
                        new DateTime(2013, 5, 16), new List<Track>()
                        {
                            new Track()
                            {
                                PlayedFirstTime = new DateTime(2013, 5, 16, 19, 16, 0),
                                Name = "Loreen - We Got The Power",
                                RelativeUrlToTrackDetails = "/utwor/154943/loreen_-_we_got_the_power",
                                TimesPlayed = 0,
                                TrackHistory = new List<TrackHistory>()
                            },
                        }
                    }
                };
            }
        }

        public static IDictionary<RadioStation, Track> ExpectedCurrentlyBroadcastedTracksOrderedByRadioStationAscending
        {
            get
            {
                return new Dictionary<RadioStation, Track>()
                {
                    {
                        new RadioStation() {Id = 0, Name = "Antyradio", IsDefault = false},
                        new Track()
                        {
                            Name = "Alice In Chains - Would?",
                            RelativeUrlToTrackDetails = "/utwor/19227/alice_in_chains_-_would",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Blue FM (Poznań)", IsDefault = false},
                        new Track()
                        {
                            Name = "Macy Gray - Beauty In The World [Radio Edit]",
                            RelativeUrlToTrackDetails = "/utwor/39513/macy_gray_-_beauty_in_the_world_radio_edit",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Eska Rock", IsDefault = false},
                        new Track()
                        {
                            Name = "Kult - Układ Zamknięty",
                            RelativeUrlToTrackDetails = "/utwor/147842/kult_-_uklad_zamkniety",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "MC Radio", IsDefault = false},
                        new Track()
                        {
                            Name = "E.gorniak, M.szczesniak - Dumka Na Dwa Serca",
                            RelativeUrlToTrackDetails = "/utwor/23988/e_gorniak_m_szczesniak_-_dumka_na_dwa_serca",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Muzyczne Radio", IsDefault = false},
                        new Track()
                        {
                            Name = "Bt Sound - Shake That Ass (Solid As A Rock) (Radio Edit)",
                            RelativeUrlToTrackDetails =
                                "/utwor/157765/bt_sound_-_shake_that_ass_solid_as_a_rock_radio_edit",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Planeta FM (Warszawa)", IsDefault = false},
                        new Track()
                        {
                            Name = "Chromatics - Lady",
                            RelativeUrlToTrackDetails = "/utwor/123653/chromatics_-_lady",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Radio Eska", IsDefault = false},
                        new Track()
                        {
                            Name = "Honey - Nie Powiem Jak",
                            RelativeUrlToTrackDetails = "/utwor/145820/honey_-_nie_powiem_jak",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Radio Express", IsDefault = false},
                        new Track()
                        {
                            Name = "Solid Base - Diamonds Are Forever",
                            RelativeUrlToTrackDetails = "/utwor/120323/solid_base_-_diamonds_are_forever",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Radio FaMa (Kielce)", IsDefault = false},
                        new Track()
                        {
                            Name = "R.i.o. - Living In Stereo",
                            RelativeUrlToTrackDetails = "/utwor/158778/r_i_o_-_living_in_stereo",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Radio GRA", IsDefault = false},
                        new Track()
                        {
                            Name = "Black Eyed Peas - I Gotta Feeling",
                            RelativeUrlToTrackDetails = "/utwor/12107/black_eyed_peas_-_i_gotta_feeling",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Radio PiN", IsDefault = false},
                        new Track()
                        {
                            Name = "Varano - Swim Into The Blue",
                            RelativeUrlToTrackDetails = "/utwor/11269/varano_-_swim_into_the_blue",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Radio Plus", IsDefault = false},
                        new Track()
                        {
                            Name = "Garou - Gitan",
                            RelativeUrlToTrackDetails = "/utwor/3139/garou_-_gitan",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Radio ZET", IsDefault = false},
                        new Track()
                        {
                            Name = "Pet Shop Boys - Domino Dancing",
                            RelativeUrlToTrackDetails = "/utwor/3119/pet_shop_boys_-_domino_dancing",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "RMF Classic", IsDefault = false},
                        new Track()
                        {
                            Name = "Georg-Philipp Telemann - Koncert Na Trąbkę D-Dur (2)",
                            RelativeUrlToTrackDetails = "/utwor/3164/georg-philipp_telemann_-_koncert_na_trabke_d-dur_2",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "RMF FM", IsDefault = false},
                        new Track()
                        {
                            Name = "Sylwia Grzeszczak - Pożyczony",
                            RelativeUrlToTrackDetails = "/utwor/158752/sylwia_grzeszczak_-_pozyczony",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "RMF MAXXX", IsDefault = false},
                        new Track()
                        {
                            Name = "Macklemore / Ryan Lewis - Can't Hold Us",
                            RelativeUrlToTrackDetails = "/utwor/145295/macklemore_ryan_lewis_-_can_t_hold_us",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Roxy FM", IsDefault = false},
                        new Track()
                        {
                            Name = "Loco Star - Tv Head",
                            RelativeUrlToTrackDetails = "/utwor/152588/loco_star_-_tv_head",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },
                    {
                        new RadioStation() {Id = 0, Name = "Złote Przeboje", IsDefault = false},
                        new Track()
                        {
                            Name = "Bajm - Piechotą Do Lata",
                            RelativeUrlToTrackDetails = "/utwor/22770/bajm_-_piechota_do_lata",
                            PlayedFirstTime = null,
                            TimesPlayed = 0,
                            TrackHistory = new List<TrackHistory>()
                        }
                    },

                };
            }
        }

        public static IEnumerable<Track> ExpectedBrodcastHistoryIn_6_6_2013_from_9_to_11
        {
            get
            {
                const int day = 6;
                const int month = 6;
                const int year = 2013;

                return new List<Track>()
                {
                    new Track()
                    {
                        Name = "Connells - 74-75",
                        RelativeUrlToTrackDetails = "/utwor/1678/connells_-_74-75",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 9, 6, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Lutricia Mcneal - You Showed Me",
                        RelativeUrlToTrackDetails = "/utwor/3377/lutricia_mcneal_-_you_showed_me",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 9, 9, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Justin Timberlake - Mirrors",
                        RelativeUrlToTrackDetails = "/utwor/141039/justin_timberlake_-_mirrors",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 9, 13, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Joan Jett / Blackhearts - I Love Rock 'n' Roll",
                        RelativeUrlToTrackDetails = "/utwor/17742/joan_jett_blackhearts_-_i_love_rock_n_roll",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 9, 21, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Labrinth / Emeli Sande - Beneath Your Beautiful",
                        RelativeUrlToTrackDetails = "/utwor/123152/labrinth_emeli_sande_-_beneath_your_beautiful",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 9, 24, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Liquido - Narcotic",
                        RelativeUrlToTrackDetails = "/utwor/1577/liquido_-_narcotic",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 9, 28, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Adele - Rolling In The Deep",
                        RelativeUrlToTrackDetails = "/utwor/68278/adele_-_rolling_in_the_deep",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 9, 36, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Killer - Liar",
                        RelativeUrlToTrackDetails = "/utwor/1986/killer_-_liar",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 9, 40, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Enej - Symetryczno-Liryczna",
                        RelativeUrlToTrackDetails = "/utwor/123003/enej_-_symetryczno-liryczna",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 9, 44, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Red Hot Chili Peppers - Californication",
                        RelativeUrlToTrackDetails = "/utwor/5998/red_hot_chili_peppers_-_californication",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 9, 51, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Passenger - Let Her Go",
                        RelativeUrlToTrackDetails = "/utwor/123001/passenger_-_let_her_go",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 9, 55, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Eros Ramazzotti - Cose Della Vita",
                        RelativeUrlToTrackDetails = "/utwor/135/eros_ramazzotti_-_cose_della_vita",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 10, 5, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Oddział Zamknięty - Obudź Się",
                        RelativeUrlToTrackDetails = "/utwor/955/oddzial_zamkniety_-_obudz_sie",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 10, 9, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Bastille - Pompeii",
                        RelativeUrlToTrackDetails = "/utwor/136911/bastille_-_pompeii",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 10, 13, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Eric Clapton - Tears In Heaven",
                        RelativeUrlToTrackDetails = "/utwor/6338/eric_clapton_-_tears_in_heaven",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 10, 22, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Loreen - Euphoria",
                        RelativeUrlToTrackDetails = "/utwor/109178/loreen_-_euphoria",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 10, 27, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Katy Perry - I Kissed A Girl",
                        RelativeUrlToTrackDetails = "/utwor/1249/katy_perry_-_i_kissed_a_girl",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 10, 29, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Lumineers - Ho Hey",
                        RelativeUrlToTrackDetails = "/utwor/124687/lumineers_-_ho_hey",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 10, 37, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Eurythmics - Sweet Dreams (Are Made Of This)",
                        RelativeUrlToTrackDetails = "/utwor/436/eurythmics_-_sweet_dreams_are_made_of_this",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 10, 39, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Avril Lavigne - Here's To Never Growing Up",
                        RelativeUrlToTrackDetails = "/utwor/148512/avril_lavigne_-_here_s_to_never_growing_up",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 10, 43, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Lady Pank - Mniej Niż Zero",
                        RelativeUrlToTrackDetails = "/utwor/523/lady_pank_-_mniej_niz_zero",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 10, 51, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Calvin Harris / Ellie Goulding - I Need Your Love",
                        RelativeUrlToTrackDetails = "/utwor/143045/calvin_harris_ellie_goulding_-_i_need_your_love",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 10, 55, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Madness - Our House",
                        RelativeUrlToTrackDetails = "/utwor/2372/madness_-_our_house",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 11, 4, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Elektryczne Gitary - Koniec",
                        RelativeUrlToTrackDetails = "/utwor/926/elektryczne_gitary_-_koniec",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 11, 7, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Lana Del Rey - Dark Paradise",
                        RelativeUrlToTrackDetails = "/utwor/115431/lana_del_rey_-_dark_paradise",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 11, 10, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Joe Cocker - Summer In The City",
                        RelativeUrlToTrackDetails = "/utwor/1147/joe_cocker_-_summer_in_the_city",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 11, 18, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Wiz Khalifa / Snoop Dogg / Bruno Mars - Young, Wild And Free",
                        RelativeUrlToTrackDetails =
                            "/utwor/104383/wiz_khalifa_snoop_dogg_bruno_mars_-_young_wild_and_free",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 11, 22, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Phil Collins - You Can't Hurry Love",
                        RelativeUrlToTrackDetails = "/utwor/3095/phil_collins_-_you_can_t_hurry_love",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 11, 25, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Yugopolis - Ostatnia Nocka",
                        RelativeUrlToTrackDetails = "/utwor/100073/yugopolis_-_ostatnia_nocka",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 11, 33, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Seal - Love's Divine",
                        RelativeUrlToTrackDetails = "/utwor/274/seal_-_love_s_divine",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 11, 37, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Naughty Boy / Sam Smith - La La La",
                        RelativeUrlToTrackDetails = "/utwor/146755/naughty_boy_sam_smith_-_la_la_la",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 11, 40, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Golden Life - 24.11.94",
                        RelativeUrlToTrackDetails = "/utwor/737/golden_life_-_24_11_94",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 11, 48, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Onerepublic - If I Lose Myself",
                        RelativeUrlToTrackDetails = "/utwor/133888/onerepublic_-_if_i_lose_myself",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 11, 52, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    },
                    new Track()
                    {
                        Name = "Bon Jovi - It's My Life",
                        RelativeUrlToTrackDetails = "/utwor/1423/bon_jovi_-_it_s_my_life",
                        PlayedFirstTime = null,
                        TrackHistory =
                            new List<TrackHistory>()
                            {
                                new TrackHistory()
                                {
                                    Broadcasted = new DateTime(year, month, day, 11, 55, 0),
                                    RadioStation = null
                                }
                            },
                        TimesPlayed = 0
                    }
                };
            }
        }

        public static IEnumerable<TrackHistory> ExpectedTrackHistory
        {
            get
            {
                return new List<TrackHistory>() 
                {
                    new TrackHistory(){ Broadcasted = new DateTime(2013, 6, 7, 17, 45, 0), RadioStation = new RadioStation(){ Name = "Radio PiN", Id  = 0, IsDefault = false }},
                    new TrackHistory(){ Broadcasted = new DateTime(2013, 6, 7, 13, 28, 0), RadioStation = new RadioStation(){ Name = "Radio PiN", Id  = 0, IsDefault = false }},
                    new TrackHistory(){ Broadcasted = new DateTime(2013, 6, 6, 19, 36, 0), RadioStation = new RadioStation(){ Name = "RMF FM", Id  = 0, IsDefault = false }},
                    new TrackHistory(){ Broadcasted = new DateTime(2013, 6, 6, 10, 41, 0), RadioStation = new RadioStation(){ Name = "Radio PiN", Id  = 0, IsDefault = false }},
                    new TrackHistory(){ Broadcasted = new DateTime(2013, 6, 5, 7, 16, 0), RadioStation = new RadioStation(){ Name = "Radio PiN", Id  = 0, IsDefault = false }},
                    new TrackHistory(){ Broadcasted = new DateTime(2013, 6, 4, 18, 33, 0), RadioStation = new RadioStation(){ Name = "Eska Rock", Id  = 0, IsDefault = false }},
                    new TrackHistory(){ Broadcasted = new DateTime(2013, 6, 4, 9, 7, 0), RadioStation = new RadioStation(){ Name = "Radio PiN", Id  = 0, IsDefault = false }},
                    new TrackHistory(){ Broadcasted = new DateTime(2013, 6, 3, 13, 43, 0), RadioStation = new RadioStation(){ Name = "Radio ZET", Id  = 0, IsDefault = false }},
                    new TrackHistory(){ Broadcasted = new DateTime(2013, 6, 3, 6, 54, 0), RadioStation = new RadioStation(){ Name = "Radio PiN", Id  = 0, IsDefault = false }},
                    new TrackHistory(){ Broadcasted = new DateTime(2013, 6, 1, 12, 4, 0), RadioStation = new RadioStation(){ Name = "Radio ZET", Id  = 0, IsDefault = false }},
                };
            }
        }
    }
}
