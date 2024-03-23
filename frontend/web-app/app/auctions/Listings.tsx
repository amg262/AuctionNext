'use client'


import {useEffect, useState} from "react";
import {useParamsStore} from "@/hooks/useParamsStore";
import {shallow} from "zustand/shallow";
import {useAuctionStore} from "@/hooks/useAuctionStore";
import {getData} from "@/app/actions/auctionActions";
import Filters from "@/app/auctions/Filters";
import EmptyFilter from "@/app/components/EmptyFilter";
import AuctionCard from "@/app/auctions/AuctionCard";
import AppPagination from "@/app/components/AppPagination";
import qs from 'query-string';


export default function Listings() {
  const [loading, setLoading] = useState(true);
  const params = useParamsStore(state => ({
    pageNumber: state.pageNumber,
    pageSize: state.pageSize,
    searchTerm: state.searchTerm,
    orderBy: state.orderBy,
    filterBy: state.filterBy,
    seller: state.seller,
    winner: state.winner,
    gridColumns: state.gridColumns
  }), shallow)
  const data = useAuctionStore(state => ({
    auctions: state.auctions,
    totalCount: state.totalCount,
    pageCount: state.pageCount
  }), shallow);
  const setData = useAuctionStore(state => state.setData);

  const setParams = useParamsStore(state => state.setParams);
  const url = qs.stringifyUrl({url: '', query: params})

  function setPageNumber(pageNumber: number) {
    setParams({pageNumber})
  }


  useEffect(() => {
    getData(url).then(data => {
      setData(data);
      setLoading(false);
    })
  }, [url, setData, params.gridColumns])

  if (loading) return <h3>Loading...</h3>

  console.log('Listings rendered', params.gridColumns)
  return (
      <>
        <Filters/>
        {data.totalCount === 0 ? (
            <EmptyFilter showReset/>
        ) : (
            <>
              {/*<div className={`grid grid-cols-4 gap-6`}> /!* Dynamic grid columns *!/*/}
              <div className={`grid grid-cols-${params.gridColumns} gap-6`}> {/* Dynamic grid columns */}
                {data.auctions.map(auction => (
                    <AuctionCard auction={auction} key={auction.id}/>
                ))}
              </div>
              <div className='flex justify-center mt-4'>
                <AppPagination pageChanged={setPageNumber}
                               currentPage={params.pageNumber} pageCount={data.pageCount}/>
              </div>
            </>
        )}

      </>

  )
}
