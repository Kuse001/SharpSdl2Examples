﻿using System;
using SDL2;

namespace SdlExample
{
    class Program
    {
        //Screen dimension constants
        private const int SCREEN_WIDTH = 640;

        private const int SCREEN_HEIGHT = 480;

        //The window we'll be rendering to
        private static IntPtr _Window = IntPtr.Zero;

        //The surface contained by the window
        private static IntPtr _ScreenSurface = IntPtr.Zero;

        private static IntPtr _CurrentSurface = IntPtr.Zero;

        //The images that correspond to a keypress
        private static readonly IntPtr[] _KeyPressSurfaces = new IntPtr[6];

        static int Main(string[] args)
        {
            //Start up SDL and create window
            if (Init() == false)
            {
                Console.WriteLine("Failed to initialize!");
            }
            else
            {
                //Load media
                if (LoadMedia() == false)
                {
                    Console.WriteLine("Failed to load media!");
                }
                else
                {
                    //Main loop flag
                    bool quit = false;

                    //Set default current surface
                    _CurrentSurface = _KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_DEFAULT];

                    //While application is running
                    while (!quit)
                    {
                        //Event handler
                        SDL.SDL_Event e;

                        //Handle events on queue
                        while (SDL.SDL_PollEvent(out e) != 0)
                        {
                            //User requests quit
                            if (e.type == SDL.SDL_EventType.SDL_QUIT)
                            {
                                quit = true;
                            }
                            //User presses a key
                            else if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                            {
                                //Select surfaces based on key press
                                switch (e.key.keysym.sym)
                                {
                                    case SDL.SDL_Keycode.SDLK_UP:
                                        _CurrentSurface = _KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_UP];
                                        break;

                                    case SDL.SDL_Keycode.SDLK_DOWN:
                                        _CurrentSurface = _KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_DOWN];
                                        break;

                                    case SDL.SDL_Keycode.SDLK_LEFT:
                                        _CurrentSurface = _KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_LEFT];
                                        break;

                                    case SDL.SDL_Keycode.SDLK_RIGHT:
                                        _CurrentSurface = _KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_RIGHT];
                                        break;

                                    default:
                                        _CurrentSurface = _KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_DEFAULT];
                                        break;
                                }
                            }
                        }

                        //Apply the current image
                        SDL.SDL_BlitSurface(_CurrentSurface, IntPtr.Zero, _ScreenSurface, IntPtr.Zero);

                        //Update the surface
                        SDL.SDL_UpdateWindowSurface(_Window);
                    }
                }
            }

            //Free resources and close SDL
            Close();

            //Console.ReadLine();
            return 0;
        }

        private static void Close()
        {
            //Deallocate surfaces
            for (int i = 0; i < _KeyPressSurfaces.Length; ++i)
            {
                SDL.SDL_FreeSurface(_KeyPressSurfaces[i]);
                _KeyPressSurfaces[i] = IntPtr.Zero;
            }

            //Destroy window
            SDL.SDL_DestroyWindow(_Window);
            _Window = IntPtr.Zero;

            //Quit SDL subsystems
            SDL.SDL_Quit();
        }

        static bool LoadMedia()
        {
            //Loading success flag
            bool success = true;

            //Load default surface
            _KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_DEFAULT] = LoadSurface("press.bmp");
            if (_KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_DEFAULT] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load default image!");
                success = false;
            }

            //Load up surface
            _KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_UP] = LoadSurface("up.bmp");
            if (_KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_UP] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load up image!");
                success = false;
            }

            //Load down surface
            _KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_DOWN] = LoadSurface("down.bmp");
            if (_KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_DOWN] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load down image!");
                success = false;
            }

            //Load left surface
            _KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_LEFT] = LoadSurface("left.bmp");
            if (_KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_LEFT] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load left image!");
                success = false;
            }

            //Load right surface
            _KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_RIGHT] = LoadSurface("right.bmp");
            if (_KeyPressSurfaces[(int)KeyPressSurfaces.KEY_PRESS_SURFACE_RIGHT] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load right image!");
                success = false;
            }

            return success;
        }

        private static IntPtr LoadSurface(string path)
        {
            //Load image at specified path
            var loadedSurface = SDL.SDL_LoadBMP(path);
            if (loadedSurface == IntPtr.Zero)
                Console.WriteLine("Unable to load image {0}! SDL Error: {1}", path, SDL.SDL_GetError());

            return loadedSurface;
        }

        private static bool Init()
        {
            //Initialization flag
            bool success = true;

            //Initialize SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("SDL could not initialize! SDL_Error: {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                //Create window
                _Window = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED,
                    SCREEN_WIDTH, SCREEN_HEIGHT, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
                if (_Window == IntPtr.Zero)
                {
                    Console.WriteLine("Window could not be created! SDL_Error: {0}", SDL.SDL_GetError());
                    success = false;
                }
                else
                {
                    //Get window surface
                    _ScreenSurface = SDL.SDL_GetWindowSurface(_Window);
                }
            }

            return success;
        }

        //Key press surfaces constants
        public enum KeyPressSurfaces
        {
            KEY_PRESS_SURFACE_DEFAULT,
            KEY_PRESS_SURFACE_UP,
            KEY_PRESS_SURFACE_DOWN,
            KEY_PRESS_SURFACE_LEFT,
            KEY_PRESS_SURFACE_RIGHT,
            KEY_PRESS_SURFACE_TOTAL
        }
    }
}
