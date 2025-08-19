using System;
using parlayrunner.Shared.Interfaces;

namespace parlayrunner.Shared.ViewModels
{
	public class ParlayViewModel
	{
		public readonly ICloudService _cloudService;
        public readonly IParlayService _parlayService;
        public ParlayViewModel(ICloudService cloudService, IParlayService parlayService)
		{
			_cloudService = cloudService;
			_parlayService = parlayService;
        }
	}
}