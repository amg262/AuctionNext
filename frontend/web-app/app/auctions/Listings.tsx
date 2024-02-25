'use client';

import * as React from 'react';
import {useEffect, useState} from 'react';
import AuctionCard from "@/app/auctions/AuctionCard";
import {Auction, PagedResult} from "@/types";
import AppPagination from "@/app/components/AppPagination";
import {getData} from "../actions/auctionActions";
import Filters from "@/app/auctions/Filters";
import {useParamsStore} from "@/hooks/useParamsStore";
import {shallow} from 'zustand/shallow';
import qs from 'query-string';

/**
 * The Listings component asynchronously fetches auction listings and renders them using AuctionCard components.
 *
 * This component makes use of the `getData` function to fetch data on mount and then renders a grid of AuctionCard
 * components, each representing an auction. The grid layout is achieved using Tailwind CSS classes.
 *
 * @return {Promise<JSX.Element>} A promise that resolves to the JSX.Element representing the listings grid.
 */
export default function Listings(): React.JSX.Element {
  // const [auctions, setAuctions] = useState<Auction[]>([]);
  // const [pageCount, setPageCount] = useState(0);
  // const [pageNumber, setPageNumber] = useState(1);
  // const [pageSize, setPageSize] = useState(4);

  const [data, setData] = useState<PagedResult<Auction>>();
  const params = useParamsStore(state => ({
    pageNumber: state.pageNumber,
    pageSize: state.pageSize,
    searchTerm: state.searchTerm,
  }), shallow)
  const setParams = useParamsStore(state => state.setParams);
  const url = qs.stringifyUrl({url: '', query: params});

  function setPageNumber(pageNumber: number) {
    setParams({pageNumber});
  }

  // // code we put inside here will load once and potentially more than once
  useEffect(() => {
    // Fetches auction data from the server and updates the state.
    getData(url).then(data => {
      setData(data);
    });
  }, [url]); // This will run once when the component mounts and then every time pageNumber changes


  if (!data) return <div>Loading...</div>;

  return (
      <>
        <Filters></Filters>
        <div className="grid grid-cols-4 gap-6">
          {data.results.map(auction => (
              <AuctionCard key={auction.id} auction={auction}/>
          ))}
        </div>
        <div className="flex justify-center mt-4 ">
          <AppPagination pageChanged={setPageNumber} currentPage={params.pageNumber} pageCount={data.pageCount}/>
        </div>
      </>

  );
}