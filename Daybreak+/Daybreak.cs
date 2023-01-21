using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockEntities.Implementations;
using ModLoaderInterfaces;
using Pipliz;

namespace Daybreak_
{
    public class Daybreak : IOnUpdate
    {
        public const string NAMESPACE = "ModEntry";
        public static string MOD_DIRECTORY;

        public const string ASSETS = "assets";

        public static float lastTime = 0;
        public void OnUpdate()
        {
            float currentTime = TimeCycle.TimeOfDayHours;

            // check daybreak
            if (System.Math.Abs(lastTime) > 0.05)
            {
                if (currentTime > TimeCycle.SunRise && lastTime <= TimeCycle.SunRise)
                {
                    //Log.Write("Playing Daybreak sounds.");

                    for (int i = 0; i < Players.ConnectedPlayers.Count; i++)
                    {
                        //Players.Player p = Players.GetConnectedByIndex(0);
                        PlayDaybreakSoundForPlayer(Players.ConnectedPlayers[i], false);
                    }
                }
                if (currentTime > TimeCycle.SunSet && lastTime <= TimeCycle.SunSet)
                {
                    //Log.Write("Playing Nightfall sounds.");

                    for (int i = 0; i < Players.ConnectedPlayers.Count; i++)
                    {
                        //Players.Player p = Players.GetConnectedByIndex(0);
                        PlayNightfallSoundForPlayer(Players.ConnectedPlayers[i], false);
                    }
                }
            }
            // check nightfall
            //TimeCycle.SunSet;

            lastTime = currentTime;
        }
        public static void PlaySoundAtBanner(BannerTracker.Banner banner, string soundname)
        {
            UnityEngine.Vector3 bannerpos = new UnityEngine.Vector3(banner.Position.x, banner.Position.y, banner.Position.z);
            AudioManager.SendAudio(bannerpos, soundname);
        }

        public static void PlayDaybreakSoundForPlayer(Players.Player player, bool OnlyActiveColony)
        {
            if (OnlyActiveColony)
            {
                try
                {
                    BannerTracker.Banner b = player.ActiveColony.Banners[0];
                    PlaySoundAtBanner(b, "daybreak");

                }
                catch (System.Exception)
                {
                    Log.Write("Couldn't play daybreak sound no active colony found");
                }
            }
            else
            {
                List<BannerTracker.Banner> bannerlist = new List<BannerTracker.Banner>();

                ServerManager.BlockEntityTracker.BannerTracker.Foreach((Vector3Int position, BannerTracker.Banner typeInstance, ref List<BannerTracker.Banner> data) => data.Add(typeInstance), ref bannerlist);

                foreach (var banner in bannerlist)
                {
                    PlaySoundAtBanner(banner, "daybreak");
                }
            }
        }
        public static void PlayNightfallSoundForPlayer(Players.Player player, bool OnlyActiveColony)
        {
            if (OnlyActiveColony)
            {
                try
                {
                    BannerTracker.Banner b = player.ActiveColony.Banners[0];
                    PlaySoundAtBanner(b, "nightfall");
                }
                catch (System.Exception)
                {
                    Log.Write("Couldn't play nightfall sound no active colony found");
                }
            }
            else
            {
                List<BannerTracker.Banner> bannerlist = new List<BannerTracker.Banner>();

                ServerManager.BlockEntityTracker.BannerTracker.Foreach((Vector3Int position, BannerTracker.Banner typeInstance, ref List<BannerTracker.Banner> data) => data.Add(typeInstance), ref bannerlist);

                foreach (var banner in bannerlist)
                {
                    PlaySoundAtBanner(banner, "nightfall");
                }
            }
        }
    }
}
