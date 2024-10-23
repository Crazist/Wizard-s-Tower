using UnityEngine;

namespace Factory
{
    public class UIFactory
    {
        private const string UIResourcePath = "UI";
        private const string JoystickResourcePath = "Joystick";
        private const string SkillsWindowPath = "SkillsWindow";

        public Transform UI { get; set; }
        public FixedJoystick FixedJoystick { get; set; }

        public void CreateUI() => 
            UI = Object.Instantiate(Resources.Load<GameObject>(UIResourcePath)).transform;
      
        public void CreateJoystick() => 
            FixedJoystick = Object.Instantiate(Resources.Load<FixedJoystick>(JoystickResourcePath), UI, false);
      
        public void CreateSkillsWindow() => 
            Object.Instantiate(Resources.Load<GameObject>(SkillsWindowPath), UI, false);
    }
}
