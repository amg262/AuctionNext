import {Bid} from "@/types"
import {create} from "zustand"

type State = {
  bids: Bid[]
  open: boolean
}

type Actions = {
  setBids: (bids: Bid[]) => void
  addBid: (bid: Bid) => void
  setOpen: (value: boolean) => void
}

/**
 * Creates a store to manage bid-related state and actions.
 * Allows setting all bids, adding a bid, and managing the visibility of bid-related components.
 */
export const useBidStore = create<State & Actions>((set) => ({
  bids: [],
  open: true,

  /**
   * Sets the entire list of bids.
   * @param bids The array of Bid objects.
   */
  setBids: (bids: Bid[]) => {
    set(() => ({
      bids
    }))
  },

  /**
   * Adds a new bid to the bid list if it doesn't already exist.
   * @param bid The Bid object to add.
   */
  addBid: (bid: Bid) => {
    set((state) => ({
      bids: !state.bids.find(x => x.id === bid.id) ? [bid, ...state.bids] : [...state.bids]
    }))
  },

  /**
   * Sets the open state, controlling visibility.
   * @param value The boolean state indicating if the component should be open.
   */
  setOpen: (value: boolean) => {
    set(() => ({
      open: value
    }))
  }
}))