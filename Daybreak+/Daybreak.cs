using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BlockEntities.Implementations;
using ModLoaderInterfaces;
using Pipliz;
using static BlockEntities.Implementations.BannerTracker;

namespace Daybreak_
{
    public class Daybreak : IOnUpdate
    {
        public static float lastTime = 0;

        List<Banner> triggeredBanners = new List<Banner>();

        public void OnUpdate()
        {
            float currentTime = TimeCycle.TimeOfDayHours;

            // check for daybreak/nightfall
            if (System.Math.Abs(lastTime) > 0.05)
            {
                if (currentTime > TimeCycle.SunRise && lastTime <= TimeCycle.SunRise)
                {
                    //Log.Write("Playing Daybreak sound at all active banners.");
                    PlaySoundAtAllActiveBanners(true);
                }
                if (currentTime > TimeCycle.SunSet && lastTime <= TimeCycle.SunSet)
                {
                    //Log.Write("Playing Nightfall sound at all active banners.");
                    PlaySoundAtAllActiveBanners(false);
                }
            }

            lastTime = currentTime;
        }

        public void PlaySoundAtAllActiveBanners(bool isDaybreakSound)
        {
            for (int i = 0; i < Players.ConnectedPlayers.Count; i++)
            {
                ServerManager.BlockEntityTracker.BannerTracker.TryGetClosest(new Vector3Int(Players.ConnectedPlayers[i].Position), out Banner banner);
                if (banner != null)
                {
                    if (isDaybreakSound)
                        PlaySoundAtBanner(banner, "daybreak");
                    else
                        PlaySoundAtBanner(banner, "nightfall");
                }
            }

            //for (int i = 0; i < Players.ConnectedPlayers.Count; i++)
            //{
            //    if (Players.ConnectedPlayers[i].ActiveColony != null)
            //    {
            //        if (isDaybreakSound)
            //            PlaySoundAtBanner(Players.ConnectedPlayers[i].ActiveColony.Banners[0], "daybreak");
            //        else
            //            PlaySoundAtBanner(Players.ConnectedPlayers[i].ActiveColony.Banners[0], "nightfall");
            //    }
            //}

            triggeredBanners.Clear();
        }

        public void PlaySoundAtBanner(BannerTracker.Banner banner, string soundname)
        {
            Log.Write("Played at banner: " + banner.Colony.Name);
            if (triggeredBanners.Contains(banner))
            {
                Log.Write("Banner already triggered");
                return;
            }

            UnityEngine.Vector3 bannerpos = new UnityEngine.Vector3(banner.Position.x, banner.Position.y, banner.Position.z);
            AudioManager.SendAudio(bannerpos, soundname);

            triggeredBanners.Add(banner);
        }
    }
}
