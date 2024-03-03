'use client';
import Countdown, {zeroPad} from "react-countdown";

type Props = {
  auctionEnd: string;
}
const renderer = ({days, hours, minutes, seconds, completed}: {
  days: number,
  hours: number,
  minutes: number,
  seconds: number,
  completed: boolean
}) => {

  return (
      <div className={`
        border-2 border-white text-white py-1 px-2 rounded-lg flex justify-center
        ${completed ? 'bg-red-500' : (days === 0 && hours < 10) ? 'bg-amber-600' : 'bg-green-600'}
      `}>

        {completed ? (
            <span>Auction Finished</span>
        ) : (
            <span suppressHydrationWarning={true}>{zeroPad(days)}:{zeroPad(hours)}:{zeroPad(minutes)}:{zeroPad(seconds)}</span>
        )}
      </div>
  )
};

export function CountdownTimer({auctionEnd}: Props) {
  return (
      <div>
        <Countdown date={auctionEnd} renderer={renderer}/>
      </div>
  );
}