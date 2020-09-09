//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using Cycle.One;
using Cycle.Three;
using Cycle.Two;
using Model;
using View;

namespace Model
{
    public interface ICar
    {
    }

    public class FastCar : ICar
    {
        public void IllegalAccess(ICanvas canvas)
        {
        }
    }

    public class SlowCar : ICar
    {
        private ICanvas _canvas;

        public SlowCar(StartCanvas canvas)
        {
            _canvas = canvas;
        }
    }

    public class SlowRocket : ICar
    {
    }

    public class StartCanvas : ICanvas
    {
    }
}

namespace View
{
    public interface ICanvas
    {
    }

    internal class EndCanvas : ICanvas
    {
    }

    [Display]
    internal class StartButton
    {
        private ICanvas _startCanvas;

        public StartButton()
        {
            _startCanvas = new StartCanvas();
        }
    }

    internal class EndButton
    {
        private ICanvas _endCanvas;

        public EndButton()
        {
            _endCanvas = new EndCanvas();
        }
    }

    public sealed class Display : Attribute
    {
    }
}

namespace Controller
{
    public class Steering
    {
        private ICar _car;

        public Steering(ICar car)
        {
            _car = car;
        }

        public void ApplySteering()
        {
        }
    }
}

namespace Cycle.One
{
    internal class CycleOneClassOne
    {
        public void CycleOneMethod()
        {
            var a = new CycleTwoClassTwo();
        }
    }

    internal class CycleOneClassTwo
    {
    }
}

namespace Cycle.Two
{
    internal class CycleTwoClassOne
    {
    }

    internal class CycleTwoClassTwo
    {
        public void CycleTwoMethod()
        {
            var a = new CycleThreeClassTwo();
        }
    }
}

namespace Cycle.Three
{
    internal class CycleThreeClassOne
    {
        public void CycleThreeMethod()
        {
            var a = new CycleOneClassOne();
        }
    }

    internal class CycleThreeClassTwo
    {
    }
}