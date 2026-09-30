// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Scoring
{
    /// <summary>
    /// Data for displaying a score breakdown at results screen.
    /// </summary>
    public class ScoreBreakdown
    {
        /// <summary>
        /// Portion of the score awarded by combo.
        /// </summary>
        public long ComboScore { get; set; }

        /// <summary>
        /// Portion of the score awarded by accuracy.
        /// </summary>
        public long AccuracyScore { get; set; }

        /// <summary>
        /// Portion of the score awarded by bonus judgements.
        /// </summary>
        public long BonusScore { get; set; }

        /// <summary>
        /// Total score before applying mod multipliers.
        /// </summary>
        public long TotalScoreWithoutMods { get; set; }

        /// <summary>
        /// Score multiplier from mods.
        /// </summary>
        public double ScoreMultiplier { get; set; }

        /// <summary>
        /// Total standardized score.
        /// </summary>
        public long TotalScore { get; set; }

        /// <summary>
        /// Classic scoring multiplier.
        /// </summary>
        public double ClassicMultiplier { get; set; }

        /// <summary>
        /// Total classic score.
        /// </summary>
        public long ClassicScore { get; set; }

        /// <summary>
        /// Create a new score breakdown.
        /// </summary>
        public ScoreBreakdown()
        {
        }
    }
}
