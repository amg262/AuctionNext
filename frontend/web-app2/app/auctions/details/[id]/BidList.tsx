'use client';

import {User} from "next-auth";
import {Auction, Bid} from "@/types";
import {useBidStore} from "@/hooks/useBidStore";
import {useEffect, useState} from "react";
import {getBidsForAuction} from "@/app/actions/auctionActions";
import Heading from "@/app/components/Heading";
import BidItem from "@/app/auctions/details/[id]/BidItem";
import {numberWithCommas} from "@/app/lib/numberWithComma";
import EmptyFilter from "@/app/components/EmptyFilter";
import BidForm from "@/app/auctions/details/[id]/BidForm";

type Props = {
  user: User | null;
  auction: Auction;
}
export default function BidList({user, auction}: Props) {
  const [loading, setLoading] = useState(true);
  const bids = useBidStore((state) => state.bids) || [];
  const setBids = useBidStore((state) => state.setBids);
  const highBid = bids.reduce((prev, current) => prev > current.amount ? prev : current.amount, 0);

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
      <div className="rounded-lg shadow-md">
        <div className="py-2 px-3 bg-white">
          <div className="sticky top-0 bg-white p-2">
            <Heading title={`Current high bid is $${numberWithCommas(highBid)}`}/>
          </div>
        </div>

        <div className="overflow-auto h-[400px] flex flex-col-reverse px-2">
          {bids.length === 0 ? (
              <EmptyFilter title="No bids yet" subtitle="Please feel free to make a bid"/>
          ) : (
              <>
                {bids.map(bid => (
                    <BidItem key={bid.id} bid={bid}/>
                ))}
              </>
          )}
        </div>

        <div className="px-2 pb-2 text-gray-500">
          <BidForm auctionId={auction.id} highBid={highBid}/>
        </div>

      </div>
  );
}