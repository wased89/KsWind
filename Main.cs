using UnityEngine;
using KsWind.Extensions;
using KSP.IO;

namespace KsWind
{
    
    public class KsWind : PartModule
    {
       
        private static Rect _windowPosition = new Rect();
        public float windSpeed = UnityEngine.Random.Range(0, 6)/10;

        public override void OnStart(StartState state)
        {

            if (state != StartState.Editor)
            {
                RenderingManager.AddToPostDrawQueue(0, OnDraw);
                
            }
            
            
        }

        public override void OnSave(ConfigNode node)
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<KsWind>();
            config.SetValue("Window Position", _windowPosition);
            config.save();
        }

        public override void OnLoad(ConfigNode node)
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<KsWind>();
            config.load();
            windSpeed = UnityEngine.Random.Range(0, 6)/10;
            _windowPosition = config.GetValue<Rect>("Window Position");
        }

        public override void OnUpdate()
        {
            if(this.vessel == FlightGlobals.ActiveVessel)
            {

                
                this.rigidbody.AddForce(windSpeed,0,0);

            }
        }

        private void OnDraw()
        {
            if (this.vessel == FlightGlobals.ActiveVessel && this.part.IsPrimary(this.vessel.parts, this.ClassID))
                 
                _windowPosition = GUILayout.Window(10, _windowPosition, OnWindow, "KsWindDetector");

            if(_windowPosition.x == 0 && _windowPosition.y == 0)
            {
                _windowPosition = _windowPosition.CenterScreen();
            }
        }

        private void OnWindow(int _windowId)
        {


            windSpeed.ToString("F2");
                GUILayout.BeginHorizontal(GUILayout.Width(250));
                GUILayout.Label("Wind Speed: " + 10 * windSpeed + "kn");
                GUILayout.EndHorizontal();
                GUI.DragWindow();
            

            
            
        }

    }
}
