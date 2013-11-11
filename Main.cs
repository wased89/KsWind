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
            windSpeed = UnityEngine.Random.Range(0, 6)/ 10.0f;
            _windowPosition = config.GetValue<Rect>("Window Position");
        }

        public override void OnUpdate()
        {
            if(this.vessel == FlightGlobals.ActiveVessel)
            {

                double Pressure = FlightGlobals.getStaticPressure(FlightGlobals.ship_altitude);
                double HighestPressure = FlightGlobals.getStaticPressure(0.0);
                double rho = FlightGlobals.getAtmDensity(Pressure);

                vesselHeight = FlightGlobals.ship_altitude;
                if(Pressure != 0)
                {
                    inAtmo = true;
                    windSpeed = TwindSpeed;
                }
                else
                {
                    inAtmo = false;
                    windSpeed = 0;
                }
                Pressure.ToString("F2");
                if (inAtmo == true)
                {
                    GUILayout.BeginHorizontal(GUILayout.Width(500));
                    GUILayout.Label("windspeed: " + (windSpeed * 10) + " knots");
                    GUILayout.Label("Vessel Altitude: " + vesselHeight);
                    GUILayout.Label("Highest Atmospheric Pressure: " + HighestPressure.ToString("0.00"));
                    GUILayout.Label("Current Atmoshperic Pressure: " + Pressure.ToString("0.00"));
                    GUILayout.EndHorizontal();
                    GUI.DragWindow();
                }
                
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

            Pressure.ToString("F2");
            if (inAtmo == true)
            {
                GUILayout.BeginHorizontal(GUILayout.Width(500));
                GUILayout.Label("windspeed: " + (windSpeed * 10) + " knots");
                GUILayout.Label("Vessel Altitude: " + vesselHeight);
                GUILayout.Label("Current Atmoshperic Pressure: " + Pressure.ToString("0.00"));
                GUILayout.Label("Highest Atmospheric Pressure: " + HighestPressure.ToString("0.00"));
                GUILayout.EndHorizontal();
                GUI.DragWindow();
            }
            else
            {
                GUILayout.BeginHorizontal(GUILayout.Width(500));
                GUILayout.Label("windspeed: " + (windSpeed * 10) + " knots");
                GUILayout.Label("Vessel Altitude: " + vesselHeight);
                GUILayout.Label("Current Atmoshperic Pressure: " + Pressure.ToString("0.00"));
                GUILayout.Label("Highest Atmospheric Pressure: " + HighestPressure.ToString("0.00"));
                GUILayout.EndHorizontal();
                GUI.DragWindow();
            }
                


            
        }

    }
}
