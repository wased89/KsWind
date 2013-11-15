
using UnityEngine;
using KsWind.Extensions;
using KSP.IO;

namespace KsWind
{
    
    public class KsWind : PartModule
    {
       
        private static Rect _windowPosition = new Rect();
        public static float windSpeed = UnityEngine.Random.Range(0, 6)/ 10.0f;
        public float TwindSpeed = windSpeed;
        public bool inAtmo = true;
        public int atmoheight = 45000;
        public double vesselHeight = 0;
        public double atmoDensity = 0;
        double Pressure = FlightGlobals.ActiveVessel.staticPressure;
        public double HighestPressure = FlightGlobals.getStaticPressure(0);
        public bool windSpeedActive;

        public override void OnStart(StartState state)
        {

            UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks; 
            if (state != StartState.Editor)
            {
                RenderingManager.AddToPostDrawQueue(0, OnDraw);
               
            }
            if (inAtmo == true)
            {
                UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks; 
                windSpeed = UnityEngine.Random.Range(0, 6) / 10.0f;
            }
            else
            {
                windSpeed = 0;
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
            
            _windowPosition = config.GetValue<Rect>("Window Position");
        }

        public override void OnUpdate()
        {
            double Pressure = FlightGlobals.getStaticPressure(FlightGlobals.ship_altitude);
            UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks; 
            if(this.vessel == FlightGlobals.ActiveVessel && windSpeedActive == true)
            {

                this.rigidbody.AddForce(windSpeed,0,0);

            }
        }

        private void OnDraw()
        {

            UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks; 

            vesselHeight = FlightGlobals.ship_altitude;
            
            if (Pressure != 0)
            {
                UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks; 
                windSpeedActive = true;
                
            }
            else
            {
                UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks; 
                windSpeedActive = false;
            }
            
            

            if (this.vessel == FlightGlobals.ActiveVessel && this.part.IsPrimary(this.vessel.parts, this.ClassID))
                 
                _windowPosition = GUILayout.Window(10, _windowPosition, OnWindow, "KsWindDetector");

            if(_windowPosition.x == 0 && _windowPosition.y == 0)
            {
                _windowPosition = _windowPosition.CenterScreen();
            }
        }

        private void OnWindow(int _windowId)
        {
            UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks; 
            double Pressure = FlightGlobals.getStaticPressure(FlightGlobals.ship_altitude);
            double HighestPressure = FlightGlobals.getStaticPressure(0.0);
            
            Pressure.ToString("F3");
            
            if (Pressure != 0)
            {
                
                GUILayout.BeginHorizontal(GUILayout.Width(600));
                GUILayout.Label("windspeed: " + (windSpeed * 10) + " knots");
                GUILayout.Label("Vessel Altitude: " + vesselHeight);
                GUILayout.Label("\rCurrent Atmoshperic Pressure: " + Pressure.ToString("0.000"));
                GUILayout.Label("Highest Atmospheric Pressure: " + HighestPressure.ToString("0.000"));
                GUILayout.Label("InAtmo? : True");
                GUILayout.EndHorizontal();
                GUI.DragWindow();
            }
            else
            {
                GUILayout.BeginHorizontal(GUILayout.Width(600));
                GUILayout.Label("windspeed: " + "0" + " knots");
                GUILayout.Label("Vessel Altitude: " + vesselHeight);
                GUILayout.Label("\rCurrent Atmoshperic Pressure: " + Pressure.ToString("0.000"));
                GUILayout.Label("Highest Atmospheric Pressure: " + HighestPressure.ToString("0.000"));
                GUILayout.Label("InAtmo? : False");
                GUILayout.EndHorizontal();
                GUI.DragWindow();
            }
            
                
            
        }

    }
}
