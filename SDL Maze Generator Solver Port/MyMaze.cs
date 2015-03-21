using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SDL2;

namespace SDL_Maze_Generator_Solver_Port
{
	enum DirEnum { DIR_LEFT, DIR_UP, DIR_RIGHT, DIR_DOWN };
	enum MoveEnum { MOVE_LEFT, MOV_FORWARD, MOV_RIGHT };

	//public struct ChoiceState
	//{
	//    //public int numChoices;
	//    public List<MoveEnum> availableChoices;
	//    //public int previousChoice;
	//}

	class MyMaze
	{
		private int _gridWidth;
		private int _gridHeight;
		private Point _offset;
		private uint _delay;

		private IntPtr _window;
		private IntPtr _renderer;

		private SDL.SDL_Event _event;

		private bool _running;
		private bool _solved;
		private bool _gaveUp;
		private bool _demoMode;

		private Point _curPlayerPos;
		private Point _prevPlayerPos;

		private bool[,] _maze;

		private Random _rand;

		public MyMaze(int width, int height, bool monitor, uint delay, bool isDemo)
		{
			// Initialize SDL
			SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);

			// Initialize Members
			SDL.SDL_Rect rect = new SDL.SDL_Rect();
			SDL.SDL_GetDisplayBounds(monitor ? 0 : 1, out rect);
			_gridWidth = (width % 2 == 1 ? width : width - 1);
			_gridHeight = (height % 2 == 1 ? height : height - 1);
			_offset = Point.Empty;
			_delay = delay;
			_demoMode = isDemo;

			int x = (rect.w / 2) - (_gridWidth * 5);
			int y = (rect.h / 2) - (_gridHeight * 5);
			_window = SDL.SDL_CreateWindow("Maze Generator/Solver", SDL.SDL_WINDOWPOS_UNDEFINED_DISPLAY(monitor ? 0 : 1), SDL.SDL_WINDOWPOS_UNDEFINED_DISPLAY(monitor ? 0 : 1), _gridWidth * 10, _gridHeight * 10, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
			_renderer = SDL.SDL_CreateRenderer(_window, -1, (uint)SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

			_running = true;

			_curPlayerPos = new Point();
			_prevPlayerPos = new Point();

			_maze = new bool[_gridWidth, _gridHeight];

			_rand = new Random();
		}

		public MyMaze(bool monitor, uint delay, bool isDemo)
		{
			// Initialize SDL
			SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);

			// Initialize members
			SDL.SDL_Rect rect = new SDL.SDL_Rect();
			SDL.SDL_GetDisplayBounds(monitor ? 0 : 1, out rect);
			_gridWidth = rect.w / 10;
			_gridHeight = rect.h / 10;
			_gridWidth = _gridWidth % 2 == 1 ? _gridWidth : _gridWidth - 1;
			_gridHeight = _gridHeight % 2 == 1 ? _gridHeight : _gridHeight - 1;
			_delay = delay;
			_demoMode = isDemo;

			// Calculate screen offsets
			_offset = new Point();
			_offset.X = (rect.w - (_gridWidth * 10)) / 2;
			_offset.Y = (rect.h - (_gridHeight * 10)) / 2;

			_window = SDL.SDL_CreateWindow("Maze Generator/Solver", rect.x, rect.y, rect.w, rect.h, SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
			_renderer = SDL.SDL_CreateRenderer(_window, -1, (uint)SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

			_running = true;

			_curPlayerPos = new Point();
			_prevPlayerPos = new Point();

			_maze = new bool[_gridWidth, _gridHeight];

			_rand = new Random();
		}
		
		private void _destroy()
		{
			SDL.SDL_DestroyWindow(_window);
			SDL.SDL_DestroyRenderer(_renderer);
			SDL.SDL_Quit();
		}

		private void _pollEvent()
		{
			while (SDL.SDL_PollEvent(out _event) > 0)
			{
				switch (_event.type)
				{
					case SDL.SDL_EventType.SDL_QUIT:
						_running = false;
						break;

					case SDL.SDL_EventType.SDL_KEYDOWN:
						if (_event.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
							_running = false;
						else if (_event.key.keysym.sym == SDL.SDL_Keycode.SDLK_TAB)
							_gaveUp = true;
						if (!_gaveUp && !_demoMode && !_solved)
						{
							switch (_event.key.keysym.sym)
							{
								case SDL.SDL_Keycode.SDLK_UP:
									_playerMove(DirEnum.DIR_UP);
									break;
								case SDL.SDL_Keycode.SDLK_DOWN:
									_playerMove(DirEnum.DIR_DOWN);
									break;
								case SDL.SDL_Keycode.SDLK_LEFT:
									_playerMove(DirEnum.DIR_LEFT);
									break;
								case SDL.SDL_Keycode.SDLK_RIGHT:
									_playerMove(DirEnum.DIR_RIGHT);
									break;
							}
						}
						break;
				}
			}
		}

		private void _drawMaze()
		{
			SDL.SDL_SetRenderDrawColor(_renderer, 0x00, 0x00, 0x00, 0xff);
			SDL.SDL_RenderClear(_renderer);
			SDL.SDL_SetRenderDrawColor(_renderer, 0xc0, 0xc0, 0xc0, 0xff);

			SDL.SDL_Rect rect = new SDL.SDL_Rect();
			rect.w = 10;
			rect.h = 10;

			for (int x = 0; x < _gridWidth; x++)
			{
				for (int y = 0; y < _gridHeight; y++)
				{
					if (!_maze[x, y])
					{
						rect.x = x * 10 + _offset.X;
						rect.y = y * 10 + _offset.Y;
						SDL.SDL_RenderFillRect(_renderer, ref rect);
					}
				}
			}

			SDL.SDL_SetRenderDrawColor(_renderer, 0x00, 0x80, 0xff, 0xff);
			rect.x = (_gridWidth - 2) * 10 + _offset.X;
			rect.y = (_gridHeight - 1) * 10 + _offset.Y;
			SDL.SDL_RenderFillRect(_renderer, ref rect);

			SDL.SDL_RenderPresent(_renderer);
		}

		private void _solve(Point current, Point previous)
		{
			// Poll events
			_pollEvent();
			if (!_running)
			{
				_delay = 0;
				return;
			}

			SDL.SDL_Rect rect = new SDL.SDL_Rect();
			rect.w = 10;
			rect.h = 10;

			// Draw the first square
			if (previous.Y == _gridHeight)
			{
				rect.x = current.X * 10 + _offset.X;
				rect.y = current.Y * 10 + _offset.Y;
				SDL.SDL_SetRenderDrawColor(_renderer, 0xff, 0x00, 0xff, 0xff);
				SDL.SDL_RenderFillRect(_renderer, ref rect);
				SDL.SDL_RenderPresent(_renderer);
				SDL.SDL_Delay(_delay);
			}

			// Calculate available choices
			Point next = Point.Empty;
			List<MoveEnum> availableChoices = new List<MoveEnum>();
			if (_calculateDir(MoveEnum.MOVE_LEFT, current, previous, ref next))
				availableChoices.Add(MoveEnum.MOVE_LEFT);
			if (_calculateDir(MoveEnum.MOV_FORWARD, current, previous, ref next))
				availableChoices.Add(MoveEnum.MOV_FORWARD);
			if (_calculateDir(MoveEnum.MOV_RIGHT, current, previous, ref next))
				availableChoices.Add(MoveEnum.MOV_RIGHT);

			// Make a move from left, then forward, then right
			while(availableChoices.Count > 0)
			{
				_calculateDir(availableChoices[0], current, previous, ref next);
				availableChoices.RemoveAt(0);
				rect.x = next.X * 10 + _offset.X;
				rect.y = next.Y * 10 + _offset.Y;
				SDL.SDL_SetRenderDrawColor(_renderer, 0xff, 0x00, 0xff, 0xff);
				SDL.SDL_RenderFillRect(_renderer, ref rect);
				SDL.SDL_RenderPresent(_renderer);
				SDL.SDL_Delay(_delay);
				if (next.X == 1 && next.Y == 0)
				{
					_solved = true;
					return;
				}
				_solve(next, current);
				if (_solved) return;
			}

			// Base Case
			if (availableChoices.Count == 0)
			{
				rect.x = current.X * 10 + _offset.X;
				rect.y = current.Y * 10 + _offset.Y;
				SDL.SDL_SetRenderDrawColor(_renderer, 0x00, 0x80, 0xff, 0xff);
				SDL.SDL_RenderFillRect(_renderer, ref rect);
				SDL.SDL_RenderPresent(_renderer);
				SDL.SDL_Delay(_delay);
				return;
			}
		}

		private void _initMaze()
		{
			_curPlayerPos.X = _gridWidth - 2;
			_curPlayerPos.Y = _gridHeight - 1;
			_prevPlayerPos.X = -1;
			_prevPlayerPos.Y = -1;
			for (int x = 0; x < _gridWidth; x++)
			{
				for (int y = 0; y < _gridHeight; y++)
				{
					_maze[x, y] = true;
				}
			}

			// Set starting and ending locations of the maze
			SDL.SDL_SetRenderDrawColor(_renderer, 0x00, 0x00, 0x00, 0xff);
			SDL.SDL_RenderClear(_renderer);
			SDL.SDL_SetRenderDrawColor(_renderer, 0xc0, 0xc0, 0xc0, 0xff);
			Point previous = new Point();
			Point current = new Point(_gridWidth - 2, _gridHeight - 1);

			_maze[current.X, current.Y] = false;

			SDL.SDL_Rect rect = new SDL.SDL_Rect();
			rect.x = current.X * 10 + _offset.X;
			rect.y = current.Y * 10 + _offset.Y;
			rect.w = 10;
			rect.h = 10;
			if (_demoMode)
			{
				SDL.SDL_RenderFillRect(_renderer, ref rect);
			}

			current.X = 1;
			current.Y = 0;

			previous.X = 1;
			previous.Y = -1;

			if (_demoMode)
			{
				rect.x = current.X * 10 + _offset.X;
				rect.y = current.Y * 10 + _offset.Y;
				SDL.SDL_RenderFillRect(_renderer, ref rect);
			}

			_maze[current.X, current.Y] = false;

			current.X = 1;
			current.Y = 1;

			if (_demoMode)
			{
				rect.x = current.X * 10 + _offset.X;
				rect.y = current.Y * 10 + _offset.Y;
				SDL.SDL_RenderFillRect(_renderer, ref rect);
			}

			_maze[current.X, current.Y] = false;

			if (_demoMode) SDL.SDL_RenderPresent(_renderer);

			_generateMaze(current, previous);

			if (_demoMode) SDL.SDL_SetRenderDrawColor(_renderer, 0x00, 0x80, 0xff, 0xff);
		}

		private void _generateMaze(Point current, Point previous)
		{
			_pollEvent();
			if (!_running)
			{
				_delay = 0;
				return;
			}


			List<Point> neighbors = new List<Point>();
			List<bool> isValid = new List<bool>();
			int numValidNeighbors = _getNeighbors(current, previous, ref neighbors, ref isValid);

			// Base Case
			if (numValidNeighbors == 0)
			{
				return;
			}

			// Every Case
			List<int> choices = new List<int>();
			SDL.SDL_Rect rect = new SDL.SDL_Rect();
			rect.w = 10;
			rect.h = 10;
			for (int i = 0; i < numValidNeighbors; i++)
			{
				// Re-evaluate neighbors
				if (i > 0)
				{
					neighbors.Clear();
					isValid.Clear();
					if (_getNeighbors(current, previous, ref neighbors, ref isValid) == 0) break;
				}

				// Choose a random valid neighbor
				int randomChoice;
				while (true)
				{
					randomChoice = _rand.Next(3);

					int j;
					for (j = 0; j <= i - 1; j++)
					{
						if (randomChoice == choices[j]) break;
					}

					if (isValid[randomChoice] && j > i - 1) break;
				}
				choices.Add(randomChoice);

				// Update our maze data and go again
				Point next = neighbors[randomChoice];
				_maze[next.X == current.X ? next.X : (next.X - current.X) / 2 + current.X, next.Y == current.Y ? next.Y : (next.Y - current.Y) / 2 + current.Y] = false;
				_maze[next.X, next.Y] = false;

				if (_demoMode)
				{
					rect.x = ((next.X - current.X) / 2 + current.X) * 10 + _offset.X;
					rect.y = ((next.Y - current.Y) / 2 + current.Y) * 10 + _offset.Y;
					SDL.SDL_RenderFillRect(_renderer, ref rect);
					SDL.SDL_RenderPresent(_renderer);
					SDL.SDL_Delay(_delay);
					rect.x = next.X * 10 + _offset.X;
					rect.y = next.Y * 10 + _offset.Y;
					SDL.SDL_RenderFillRect(_renderer, ref rect);
					SDL.SDL_RenderPresent(_renderer);
					SDL.SDL_Delay(_delay);
				}

				_generateMaze(next, current);
			}
		}

		private bool _pointInBounds(Point point, Point lower, Point upper)
		{
			if (point.X <= lower.X || point.X >= upper.X || point.Y <= lower.Y || point.Y >= upper.Y)
				return false;
			return true;
		}

		private int _getNeighbors(Point current, Point previous, ref List<Point> neighbors, ref List<bool> isValid)
		{
			Point up = new Point();
			Point down = new Point();
			Point left = new Point();
			Point right = new Point();
			up.X = current.X;
			up.Y = current.Y - 2;
			down.X = current.X;
			down.Y = current.Y + 2;
			left.X = current.X - 2;
			left.Y = current.Y;
			right.X = current.X + 2;
			right.Y = current.Y;

			if (up.X != previous.X || up.Y != previous.Y) neighbors.Add(up);
			if (down.X != previous.X || down.Y != previous.Y) neighbors.Add(down);
			if (left.X != previous.X || left.Y != previous.Y) neighbors.Add(left);
			if (right.X != previous.X || right.Y != previous.Y) neighbors.Add(right);

			Point lowerBound = new Point(0, 0);
			Point upperBound = new Point(_gridWidth - 1, _gridHeight - 1);

			foreach (Point p in neighbors)
			{
				// In bounds?
				if (!_pointInBounds(p, lowerBound, upperBound))
				{
					isValid.Add(false);
					continue;
				}//else isValid.Add(false);

				// Not already used?
				if (_maze[p.X, p.Y]) isValid.Add(true);
				else isValid.Add(false);
			}

			int numValidNeighbors = 0;
			foreach (bool b in isValid)
			{
				if (b) numValidNeighbors++;
			}

			return numValidNeighbors;
		}

		private void _playerMove(DirEnum direction)
		{
			SDL.SDL_Rect rect = new SDL.SDL_Rect();
			rect.w = 10;
			rect.h = 10;
			if (_prevPlayerPos.X == -1 && _prevPlayerPos.Y == -1 && direction == DirEnum.DIR_UP)
			{
				SDL.SDL_SetRenderDrawColor(_renderer, 0xff, 0x80, 0x00, 0xff);
				rect.x = _curPlayerPos.X * 10 + _offset.X;
				rect.y = _curPlayerPos.Y * 10 + _offset.Y;
				SDL.SDL_RenderFillRect(_renderer, ref rect);
				_prevPlayerPos = _curPlayerPos;
				_curPlayerPos.Y--;
				rect.x = _curPlayerPos.X * 10 + _offset.X;
				rect.y = _curPlayerPos.Y * 10 + _offset.Y;
				SDL.SDL_SetRenderDrawColor(_renderer, 0x00, 0x80, 0xff, 0xff);
				SDL.SDL_RenderFillRect(_renderer, ref rect);
				SDL.SDL_RenderPresent(_renderer);
				return;
			}

			Point next = _curPlayerPos;
			switch (direction)
			{
				case DirEnum.DIR_UP:
					next.Y--;
					break;
				case DirEnum.DIR_DOWN:
					next.Y++;
					break;
				case DirEnum.DIR_LEFT:
					next.X--;
					break;
				case DirEnum.DIR_RIGHT:
					next.X++;
					break;
			}

			Point upper = new Point(_gridWidth - 1, _gridHeight);
			Point lower = new Point(0, -1);

			if(!_pointInBounds(next, lower, upper)) return;

			if (_maze[next.X, next.Y]) return;

			SDL.SDL_SetRenderDrawColor(_renderer, 0xff, 0x80, 0x00, 0xff);
			rect.x = _curPlayerPos.X * 10 + _offset.X;
			rect.y = _curPlayerPos.Y * 10 + _offset.Y;
			SDL.SDL_RenderFillRect(_renderer, ref rect);
			_prevPlayerPos = _curPlayerPos;
			_curPlayerPos = next;
			rect.x = _curPlayerPos.X * 10 + _offset.X;
			rect.y = _curPlayerPos.Y * 10 + _offset.Y;
			SDL.SDL_SetRenderDrawColor(_renderer, 0x00, 0x80, 0xff, 0xff);
			SDL.SDL_RenderFillRect(_renderer, ref rect);
			SDL.SDL_RenderPresent(_renderer);

			if (next.X == 1 && next.Y == 0) _solved = true;
		}

		private bool _calculateDir(MoveEnum direction, Point current, Point previous, ref Point output)
		{
			// Calculate which direction we are moving
			DirEnum movingDir = DirEnum.DIR_DOWN;
			Point tmp = new Point(current.X - previous.X, current.Y - previous.Y);
			if (tmp.X < 0) movingDir = DirEnum.DIR_LEFT;
			if (tmp.X > 0) movingDir = DirEnum.DIR_RIGHT;
			if (tmp.Y < 0) movingDir = DirEnum.DIR_UP;
			if (tmp.Y > 0) movingDir = DirEnum.DIR_DOWN;

			// Create the offset Point
			switch (movingDir)
			{
				case DirEnum.DIR_LEFT:
					switch (direction)
					{
						case MoveEnum.MOVE_LEFT:
							tmp.X = current.X;
							tmp.Y = current.Y + 1;
							break;
						case MoveEnum.MOV_FORWARD:
							tmp.X = current.X - 1;
							tmp.Y = current.Y;
							break;
						case MoveEnum.MOV_RIGHT:
							tmp.X = current.X;
							tmp.Y = current.Y - 1;
							break;
					}
					break;

				case DirEnum.DIR_RIGHT:
					switch (direction)
					{
						case MoveEnum.MOVE_LEFT:
							tmp.X = current.X;
							tmp.Y = current.Y - 1;
							break;
						case MoveEnum.MOV_FORWARD:
							tmp.X = current.X + 1;
							tmp.Y = current.Y;
							break;
						case MoveEnum.MOV_RIGHT:
							tmp.X = current.X;
							tmp.Y = current.Y + 1;
							break;
					}
					break;
				case DirEnum.DIR_UP:
					switch (direction)
					{
						case MoveEnum.MOVE_LEFT:
							tmp.X = current.X - 1;
							tmp.Y = current.Y;
							break;
						case MoveEnum.MOV_FORWARD:
							tmp.X = current.X;
							tmp.Y = current.Y - 1;
							break;
						case MoveEnum.MOV_RIGHT:
							tmp.X = current.X + 1;
							tmp.Y = current.Y;
							break;
					}
					break;
				case DirEnum.DIR_DOWN:
					switch (direction)
					{
						case MoveEnum.MOVE_LEFT:
							tmp.X = current.X + 1;
							tmp.Y = current.Y;
							break;
						case MoveEnum.MOV_FORWARD:
							tmp.X = current.X;
							tmp.Y = current.Y + 1;
							break;
						case MoveEnum.MOV_RIGHT:
							tmp.X = current.X - 1;
							tmp.Y = current.Y;
							break;
					}
					break;
			}

			if (_maze[tmp.X, tmp.Y]) return false;

			output.X = tmp.X;
			output.Y = tmp.Y;
			return true;
		}

		public void start()
		{
			while (true)
			{
				_initMaze();
				_solved = false;
				_gaveUp = false;
				if (!_running) break;
				if (!_demoMode) _drawMaze();
				SDL.SDL_Delay(1000);

				while (!_solved)
				{
					_pollEvent();
					if (!_running) break;

					if (_gaveUp || _demoMode)
					{
						_solve(new Point(_gridWidth - 2, _gridHeight - 1), new Point(_gridWidth - 2, _gridHeight));
						break;
					}

					SDL.SDL_Delay(_delay);
				}

				if (!_running) break;
				SDL.SDL_Delay(3000);
			}

			this._destroy();
		}
	}
}
