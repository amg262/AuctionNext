import React from 'react';

type Props = {
  auction: any;
}

export default function AuctionCard({auction}: Props) {
  return (
      <div>{auction.make} {auction.model}</div>
  );
}