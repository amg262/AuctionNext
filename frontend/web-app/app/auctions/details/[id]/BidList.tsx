'use client';

import {User} from "next-auth";
import {Auction, Bid} from "@/types";
import {useBidStore} from "@/hooks/useBidStore";
import {useEffect, useState} from "react";
import {getBidsForAuction} from "@/app/actions/auctionActions";
import Heading from "@/app/components/Heading";
import BidItem from "@/app/auctions/details/[id]/BidItem";

type Props = {
  user: User | null;
  auction: Auction;
}
export default function BidList({user, auction}: Props) {
  const [loading, setLoading] = useState(true);
  const bids = useBidStore((state) => state.bids);
  const setBids = useBidStore((state) => state.setBids);

  useEffect(() => {
    // Fetches auction data from the server and updates the state.
    getBidsForAuction(auction.id)
        .then((res: any) => {
          if (res.error) throw res.error;
          setBids(res as Bid[]);
        }).catch((e) => {
    }).finally(() => setLoading(false));
  }, [auction.id, setLoading, setBids]);

  if (loading) return <div>Loading bids...</div>;
  return (
      <div className="border-2 rounded-lg p-2 bg-gray-100">
        <Heading title="Bids"/>
        {bids.map(bid => (
            <BidItem key={bid.id} bid={bid}/>
        ))}
      </div>
  );
}