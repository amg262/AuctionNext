import {Auction, PagedResult} from "@/types"
import {create} from "zustand"

type State = {
  auctions: Auction[]
  totalCount: number
  pageCount: number
}

type Actions = {
  setData: (data: PagedResult<Auction>) => void
  setCurrentPrice: (auctionId: string, amount: number) => void
}

const initialState: State = {
  auctions: [],
  pageCount: 0,
  totalCount: 0
}
/**
 * Creates a store to manage auction-related state and actions.
 * Allows setting auction data and updating the current high bid for specific auctions.
 */
export const useAuctionStore = create<State & Actions>((set) => ({
  ...initialState,

  /**
   * Sets the auction data from a paginated result.
   * @param data The paginated result containing auctions.
   */
  setData: (data: PagedResult<Auction>) => {
    set(() => ({
      auctions: data.results,
      totalCount: data.totalCount,
      pageCount: data.pageCount
    }))
  },

  /**
   * Updates the current high bid amount for a specified auction.
   * @param auctionId The unique identifier for the auction.
   * @param amount The new high bid amount.
   */
  setCurrentPrice: (auctionId: string, amount: number) => {
    set((state) => ({
      auctions: state.auctions.map((auction) => auction.id === auctionId
          ? {...auction, currentHighBid: amount} : auction)
    }))
  }
}))