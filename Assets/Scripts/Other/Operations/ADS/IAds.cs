

namespace Ads{
	
	public interface IAds{
		
		bool ShowSimple();

		bool ShowReward( System.Action success, System.Action skipped = null, System.Action failed = null );

	}

}