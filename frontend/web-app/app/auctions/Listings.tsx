import * as React from 'react';
import AuctionCard from "@/app/auctions/AuctionCard";

async function getData() {
  const res = await fetch('http://localhost:6001/search?pageSize=10');

  if (!res.ok) throw new Error('Failed to fetch data');

  return res.json();
}

export default async function Listings() {
  const data = await getData();

  return (
      <div className="grid grid-cols-4 gap-6">
        {data && data.results.map((auction: any) => (
            <AuctionCard key={auction.id} auction={auction}/>
        ))}
        {/*{JSON.stringify(data, null, 2)}*/}
      </div>
  );
}