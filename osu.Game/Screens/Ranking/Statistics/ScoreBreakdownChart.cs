// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Extensions.LocalisationExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Utils;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osuTK.Graphics;

namespace osu.Game.Screens.Ranking.Statistics
{
    public partial class ScoreBreakdownChart : Container
    {
        private readonly ScoreInfo score;

        private Drawable spinner = null!;
        private Drawable content = null!;
        private OsuSpriteText comboScore = null!;
        private OsuSpriteText accuracyScore = null!;
        private OsuSpriteText bonusScore = null!;
        private OsuSpriteText totalScoreWithoutMods = null!;
        private OsuSpriteText scoreMultiplier = null!;
        private OsuSpriteText totalScore = null!;
        private OsuSpriteText classicMultiplier = null!;
        private OsuSpriteText classicScore = null!;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        [Resolved]
        private BeatmapDifficultyCache difficultyCache { get; set; } = null!;

        public ScoreBreakdownChart(ScoreInfo score)
        {
            this.score = score;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new[]
            {
                spinner = new LoadingSpinner(true)
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre
                },
                content = new GridContainer
                {
                    Alpha = 0,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Origin = Anchor.TopCentre,
                    Anchor = Anchor.TopCentre,
                    RowDimensions = new[] { new Dimension(GridSizeMode.AutoSize) },
                    ColumnDimensions = new[]
                    {
                        new Dimension(),
                        new Dimension(GridSizeMode.Absolute, 50),
                        new Dimension()
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new GridContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Origin = Anchor.Centre,
                                Anchor = Anchor.Centre,
                                ColumnDimensions = new[]
                                {
                                    new Dimension(GridSizeMode.Absolute, 30),
                                },
                                RowDimensions = new[]
                                {
                                    new Dimension(GridSizeMode.AutoSize),
                                    new Dimension(GridSizeMode.AutoSize),
                                    new Dimension(GridSizeMode.AutoSize),
                                    new Dimension(GridSizeMode.AutoSize),
                                    new Dimension(GridSizeMode.AutoSize),
                                    new Dimension(GridSizeMode.AutoSize),
                                    new Dimension(GridSizeMode.AutoSize),
                                    new Dimension(GridSizeMode.AutoSize)
                                },
                                Content = new[]
                                {
                                    new Drawable[]
                                    {
                                        new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreLeft,
                                            Anchor = Anchor.CentreLeft,
                                            Font = OsuFont.GetFont(weight: FontWeight.Regular, size: StatisticItem.FONT_SIZE),
                                            Text = "Combo Score",
                                            Colour = Color4.White
                                        },
                                        comboScore = new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreRight,
                                            Anchor = Anchor.CentreRight,
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: StatisticItem.FONT_SIZE),
                                            Colour = Color4.White
                                        },
                                    },
                                    new Drawable[]
                                    {
                                        new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreLeft,
                                            Anchor = Anchor.CentreLeft,
                                            Font = OsuFont.GetFont(weight: FontWeight.Regular, size: StatisticItem.FONT_SIZE),
                                            Text = "Accuracy Score",
                                            Colour = Color4.White
                                        },
                                        accuracyScore = new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreRight,
                                            Anchor = Anchor.CentreRight,
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: StatisticItem.FONT_SIZE),
                                            Colour = Color4.White
                                        },
                                    },
                                    new Drawable[]
                                    {
                                        new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreLeft,
                                            Anchor = Anchor.CentreLeft,
                                            Font = OsuFont.GetFont(weight: FontWeight.Regular, size: StatisticItem.FONT_SIZE),
                                            Text = "Bonus Score",
                                            Colour = Color4.White
                                        },
                                        bonusScore = new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreRight,
                                            Anchor = Anchor.CentreRight,
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: StatisticItem.FONT_SIZE),
                                            Colour = Color4.White
                                        },
                                    },
                                    new Drawable[]
                                    {
                                        new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreLeft,
                                            Anchor = Anchor.CentreLeft,
                                            Font = OsuFont.GetFont(weight: FontWeight.Regular, size: StatisticItem.FONT_SIZE),
                                            Text = "Raw Total Score",
                                            Colour = Color4.White
                                        },
                                        totalScoreWithoutMods = new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreRight,
                                            Anchor = Anchor.CentreRight,
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: StatisticItem.FONT_SIZE),
                                            Colour = Color4.White
                                        },
                                    },
                                    new Drawable[]
                                    {
                                        new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreLeft,
                                            Anchor = Anchor.CentreLeft,
                                            Font = OsuFont.GetFont(weight: FontWeight.Regular, size: StatisticItem.FONT_SIZE),
                                            Text = "Score Multiplier",
                                            Colour = Color4.White
                                        },
                                        scoreMultiplier = new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreRight,
                                            Anchor = Anchor.CentreRight,
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: StatisticItem.FONT_SIZE),
                                            Colour = Color4.White
                                        },
                                    },
                                    new Drawable[]
                                    {
                                        new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreLeft,
                                            Anchor = Anchor.CentreLeft,
                                            Font = OsuFont.GetFont(weight: FontWeight.Regular, size: StatisticItem.FONT_SIZE),
                                            Text = "Total Score",
                                            Colour = Color4.White
                                        },
                                        totalScore = new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreRight,
                                            Anchor = Anchor.CentreRight,
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: StatisticItem.FONT_SIZE),
                                            Colour = Color4.White
                                        },
                                    },
                                    new Drawable[]
                                    {
                                        new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreLeft,
                                            Anchor = Anchor.CentreLeft,
                                            Font = OsuFont.GetFont(weight: FontWeight.Regular, size: StatisticItem.FONT_SIZE),
                                            Text = "Classic Multiplier",
                                            Colour = Color4.White
                                        },
                                        classicMultiplier = new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreRight,
                                            Anchor = Anchor.CentreRight,
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: StatisticItem.FONT_SIZE),
                                            Colour = Color4.White
                                        },
                                    },
                                    new Drawable[]
                                    {
                                        new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreLeft,
                                            Anchor = Anchor.CentreLeft,
                                            Font = OsuFont.GetFont(weight: FontWeight.Regular, size: StatisticItem.FONT_SIZE),
                                            Text = "Classic Score",
                                            Colour = Color4.White
                                        },
                                        classicScore = new OsuSpriteText
                                        {
                                            Origin = Anchor.CentreRight,
                                            Anchor = Anchor.CentreRight,
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: StatisticItem.FONT_SIZE),
                                            Colour = Color4.White
                                        },
                                    },
                                }
                            },
                        }
                    }
                }
            };

            spinner.Show();

            computePerformance(cancellationTokenSource.Token)
                .ContinueWith(t => Schedule(() =>
                {
                    if (t.GetResultSafely() is ScoreBreakdown breakdown)
                        setPerformance(breakdown);
                }), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private async Task<ScoreBreakdown?> computePerformance(CancellationToken token)
        {
            var scoreProcessor = score.Ruleset.CreateInstance().CreateScoreProcessor();
            Debug.Assert(scoreProcessor != null);
            if (scoreProcessor == null)
                return null;

            return await scoreProcessor.CalculateAsync(score, token).ConfigureAwait(false);
        }

        private void setPerformance(ScoreBreakdown breakdown)
        {
            spinner.Hide();
            content.FadeIn(200);

            setTotalValues(breakdown);
        }

        private void setTotalValues(ScoreBreakdown breakdown)
        {
            comboScore.Text = breakdown.ComboScore.ToLocalisableString(@"N0");
            accuracyScore.Text = breakdown.AccuracyScore.ToLocalisableString(@"N0");
            bonusScore.Text = breakdown.BonusScore.ToLocalisableString(@"N0");
            totalScoreWithoutMods.Text = breakdown.TotalScoreWithoutMods.ToLocalisableString(@"N0");
            scoreMultiplier.Text = breakdown.ScoreMultiplier.ToLocalisableString("0.00×");
            totalScore.Text = breakdown.TotalScore.ToLocalisableString(@"N0");
            classicMultiplier.Text = breakdown.ClassicMultiplier.ToLocalisableString("0.00×");
            classicScore.Text = breakdown.ClassicScore.ToLocalisableString(@"N0");
        }

        protected override void Dispose(bool isDisposing)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();

            base.Dispose(isDisposing);
        }
    }
}
