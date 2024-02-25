'use client';

import * as React from 'react';
import {useEffect, useState} from 'react';
import AuctionCard from "@/app/auctions/AuctionCard";
import {Auction} from "@/types";
import {AppPagination} from "@/app/components/AppPagination";
import {getData} from "../actions/auctionActions";


/**
 * The Listings component asynchronously fetches auction listings and renders them using AuctionCard components.
 *
 * This component makes use of the `getData` function to fetch data on mount and then renders a grid of AuctionCard
 * components, each representing an auction. The grid layout is achieved using Tailwind CSS classes.
 *
 * @return {Promise<JSX.Element>} A promise that resolves to the JSX.Element representing the listings grid.
 */
export default function Listings(): React.JSX.Element {
  const [auctions, setAuctions] = useState<Auction[]>([]);
  const [pageCount, setPageCount] = useState(0);
  const [pageNumber, setPageNumber] = useState(1);

  // // code we put inside here will load once and potentially more than once
  useEffect(() => {
    // Fetches auction data from the server and updates the state.
    getData(pageNumber).then(data => {
      setAuctions(data.results);
      setPageCount(data.pageCount);
    });
  }, [pageNumber]); // This will run once when the component mounts and then every time pageNumber changes


  if (auctions.length === 0) return <div>Loading...</div>;

  return (
      <>
        <div className="grid grid-cols-4 gap-6">
          {auctions.map(auction => (
              <AuctionCard key={auction.id} auction={auction}/>
          ))}
        </div>
        <div className="flex justify-center mt-4 ">
          <AppPagination pageChanged={setPageNumber} currentPage={pageNumber} pageCount={pageCount}/>
        </div>
      </>

  );
}