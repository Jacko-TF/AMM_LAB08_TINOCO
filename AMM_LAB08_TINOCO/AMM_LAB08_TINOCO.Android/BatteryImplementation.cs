using AMM_LAB08_TINOCO.Droid;
using AMM_LAB08_TINOCO.interfaces;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(BatteryImplementation))]
namespace AMM_LAB08_TINOCO.Droid
{
    public class BatteryImplementation : IBattery
    {
        public BatteryImplementation() { }

        public int RemainingChargePercent
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
                            var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);

                            return (int)Math.Floor(level * 100D / scale);
                        }
                    }

                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public interfaces.BatteryStatus Status
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)interfaces.BatteryStatus.Charging || status == (int)interfaces.BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)BatteryPlugged.Ac;
                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

                            isCharging = (usbCharge || acCharge || wirelessCharge);
                            if (isCharging)
                                return interfaces.BatteryStatus.Charging;

                            switch (status)
                            {
                                case (int)interfaces.BatteryStatus.Charging:
                                    return interfaces.BatteryStatus.Charging;
                                case (int)interfaces.BatteryStatus.Discharging:
                                    return interfaces.BatteryStatus.Discharging;
                                case (int)interfaces.BatteryStatus.Full:
                                    return interfaces.BatteryStatus.Full;
                                case (int)interfaces.BatteryStatus.NotCharging:
                                    return interfaces.BatteryStatus.NotCharging;
                                default:
                                    return interfaces.BatteryStatus.Unknown;
                            }
                        }
                    }

                }catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public PowerSource PowerSource
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)interfaces.BatteryStatus.Charging || status == (int)interfaces.BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)BatteryPlugged.Ac;

                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

                            isCharging = (usbCharge || acCharge || wirelessCharge);

                            if (!isCharging)
                                return interfaces.PowerSource.Battery;
                            else if (usbCharge)
                                return interfaces.PowerSource.Usb;
                            else if (acCharge)
                                return interfaces.PowerSource.Ac;
                            else if (wirelessCharge)
                                return interfaces.PowerSource.Wireless;
                            else
                                return interfaces.PowerSource.Other;
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

    }
}