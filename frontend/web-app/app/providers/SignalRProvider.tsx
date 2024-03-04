'use client'

import {useAuctionStore} from '@/hooks/useAuctionStore';
import {useBidStore} from '@/hooks/useBidStore';
import {Auction, AuctionFinished, Bid} from '@/types';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr'
import {User} from 'next-auth';
import React, {ReactNode, useEffect, useState} from 'react'
import {toast} from 'react-hot-toast';
import AuctionCreatedToast from '../components/AuctionCreatedToast';
import {getDetailedViewData} from '../actions/auctionActions';
import AuctionFinishedToast from '../components/AuctionFinishedToast';

/**
 * Props for the SignalRProvider component.
 *
 * @type {Object} Props
 * @property {ReactNode} children - The child components to be rendered within the SignalRProvider.
 * @property {User | null} user - The current user, if authenticated, or null otherwise.
 */
type Props = {
  children: ReactNode
  user: User | null
}

/**
 * A React component that establishes a SignalR connection and listens for server-side events
 * related to auctions and bids. It provides a context for child components to interact with
 * real-time updates such as new bids and auction status changes.
 *
 * @param {Props} props - The props for the SignalRProvider component.
 * @returns {ReactNode} The children elements to be rendered.
 */
export default function SignalRProvider({children, user}: Props): ReactNode {
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const setCurrentPrice = useAuctionStore(state => state.setCurrentPrice);
  const addBid = useBidStore(state => state.addBid);

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
        .withUrl(process.env.NEXT_PUBLIC_NOTIFY_URL as string)
        .withAutomaticReconnect()
        .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection.start()
          .then(() => {
            console.log('Connected to notification hub');

            connection.on('BidPlaced', (bid: Bid) => {
              if (bid.bidStatus.includes('Accepted')) {
                setCurrentPrice(bid.auctionId, bid.amount);
              }
              addBid(bid);
            });

            connection.on('AuctionCreated', (auction: Auction) => {
              if (user?.username !== auction.seller) {
                return toast(<AuctionCreatedToast auction={auction}/>,
                    {duration: 10000})
              }
            });

            connection.on('AuctionFinished', (finishedAuction: AuctionFinished) => {
              const auction = getDetailedViewData(finishedAuction.auctionId);
              return toast.promise(auction, {
                loading: 'Loading',
                success: (auction) =>
                    <AuctionFinishedToast
                        finishedAuction={finishedAuction}
                        auction={auction}
                    />,
                error: (err) => 'Auction finished!'
              }, {success: {duration: 10000, icon: null}})
            })


          }).catch(error => console.log(error));
    }

    return () => {
      connection?.stop();
    }
  }, [addBid, connection, setCurrentPrice, user?.username])

  return (
      children
  )
}
