// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Framework.Utils;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Play.HUD;

namespace osu.Game.Tests.Visual.Gameplay
{
    [TestFixture]
    public partial class TestSceneArgonSongProgressGraph : OsuTestScene
    {
        private TestArgonSongProgressGraph? graph;

        [SetUpSteps]
        public void SetupSteps()
        {
            AddStep("add new big graph", () =>
            {
                if (graph != null)
                {
                    graph.Expire();
                    graph = null;
                }

                Add(graph = new TestArgonSongProgressGraph
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 200,
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                });
            });
        }

        [Test]
        public void Test()
        {
            AddAssert("ensure not created", () => graph!.CreationCount == 0);
            AddStep("display values", displayRandomValues);
        }

        private void displayRandomValues()
        {
            Debug.Assert(graph != null);
            var objects = new List<HitObject>();
            for (double i = 0; i < 5000; i += RNG.NextDouble() * 10 + i / 1000)
                objects.Add(new HitObject { StartTime = i });

            graph.Objects = objects;
        }

        private partial class TestArgonSongProgressGraph : ArgonSongProgressGraph
        {
            public int CreationCount { get; private set; }

            protected override void RecreateGraph()
            {
                base.RecreateGraph();
                CreationCount++;
            }
        }
    }
}
