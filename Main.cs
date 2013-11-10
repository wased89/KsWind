using UnityEngine;
using KsWind.Extensions;
using KSP.IO;

namespace KsWind
{
    [KSPAddon(KSPAddon.Startup.Flight, true)]
    public class KsWind : PartModule
    {
        public float windSpeed = UnityEngine.Random.Range(1, 4);
        private static Rect _windowPosition = new Rect();

        public override void OnStart(StartState state)
        {
            
            if (state != StartState.Editor)
                RenderingManager.AddToPostDrawQueue(0, OnDraw);
                
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
            _windowPosition = config.GetValue<Rect>("Window Position");
        }

        public override void OnUpdate()
        {
            if(this.vessel == FlightGlobals.ActiveVessel)
            {

                float windSpeed = UnityEngine.Random.Range(0, 40)/10;
                this.rigidbody.AddForce(Vector3.right * windSpeed);

                
                GUILayout.BeginHorizontal(GUILayout.Width(250));
                GUILayout.Label("Wind Speed: " + windSpeed + "kn");
                GUILayout.EndHorizontal();
                GUI.DragWindow();
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

            GUILayout.BeginHorizontal(GUILayout.Width(250));
            GUILayout.Label("Wind Speed: " + windSpeed + "kn");
            GUILayout.EndHorizontal();
            GUI.DragWindow();
               

            
        }

    }
}
