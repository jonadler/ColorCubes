﻿using System;

namespace TriangleRender
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Triangle(false))
            {
                game.Run();
            }
        }
    }
#endif
}
