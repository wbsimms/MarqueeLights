using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace MarqueeLights
{
    public partial class Program
    {
        private int toDisplay = 0;
        private GT.SocketInterfaces.DigitalOutput latch, clock, data;
        GT.Timer runntimer = new GT.Timer(75);
        void ProgramStarted()
        {
            clock = extender.CreateDigitalOutput(GT.Socket.Pin.Four, false);
            latch = extender.CreateDigitalOutput(GT.Socket.Pin.Five, false);
            data = extender.CreateDigitalOutput(GT.Socket.Pin.Six, false);
            button.ButtonPressed += button_ButtonPressed;
            runntimer.Tick += runntimer_Tick;

            for (int i = 0; i <= 15; i++)
            {
                data.Write(false);
                Clock();
            }
            Latch();
        }

        void runntimer_Tick(GT.Timer timer)
        {
            latch.Write(false);
            int copy = toDisplay;
            for (int i = 0; i <= 15; i++)
            {
                int b = copy & 1;
                if (b == 0)
                {
                    data.Write(false);
                    Clock();
                }
                else
                {
                    data.Write(true);
                    Clock();
                }
                copy = copy >> 1;
            }
            Latch();
            toDisplay = toDisplay >> 1;
        }

        void Latch()
        {
            latch.Write(true);
            latch.Write(false);
        }
        void Clock()
        {
            clock.Write(true);
            clock.Write(false);
        }

        private bool isStarted = false;
        private void button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            toDisplay = toDisplay + 0x8000;
            if (!isStarted)
            {
                runntimer.Start();
                isStarted =! isStarted;
            }
        }
    }
}
