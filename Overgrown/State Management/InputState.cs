using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Overgrown.State_Management
{
    public class InputState
    {
        public KeyboardState CurrentKeyboardState;

        private KeyboardState _previousKeyboardState;

        public MouseState CurrentMouseState;

        private MouseState _previousMouseState;

        public InputState()
        {
            CurrentKeyboardState = new KeyboardState();

            _previousKeyboardState = new KeyboardState();

            CurrentMouseState = new MouseState();

            _previousMouseState = new MouseState();
        }

        public void Update()
        {
            _previousKeyboardState = CurrentKeyboardState;
            _previousMouseState = CurrentMouseState;

            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
        }

        public bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        public bool IsNewKeyPress(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyReleased(Keys key) 
        {
            return _previousKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key);
        }

        public bool isMouseLeftClickPressed()
        {
            return CurrentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool isNewMouseLeftClickPress()
        {
            return CurrentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed;
        }

        public Vector2 GetAdjustedMouseLocation(Matrix scaleMatrix)
        {
            var inverseScaleMatrix = Matrix.Invert(scaleMatrix);

            var mouseScreenPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            var mouseGamePos = Vector2.Transform(mouseScreenPos, inverseScaleMatrix);

            return mouseGamePos;
        }
    }
}
