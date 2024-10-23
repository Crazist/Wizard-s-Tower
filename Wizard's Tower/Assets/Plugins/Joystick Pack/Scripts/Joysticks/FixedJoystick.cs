using UnityEngine;

public class FixedJoystick : Joystick
{
    private void Update()
    {
        // Если это редактор или ПК-платформа, обрабатываем ввод с клавиш WASD
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        HandleKeyboardInput();
#endif
    }

    private void HandleKeyboardInput()
    {
        // Проверка нажатия клавиш WASD и их запись в Horizontal и Vertical
        float keyboardHorizontal = Input.GetAxis("Horizontal"); // A и D
        float keyboardVertical = Input.GetAxis("Vertical"); // W и S

        if (Mathf.Abs(keyboardHorizontal) > 0.1f || Mathf.Abs(keyboardVertical) > 0.1f)
        {
            // Обновляем значения Horizontal и Vertical при наличии ввода с клавиатуры
            input = new Vector2(keyboardHorizontal, keyboardVertical);
        }
    }
}