import {Bid} from "@/types";
import {createWithEqualityFn} from "zustand/traditional";

type State = {
  bids: Bid[];
}

type Actions = {
  setBids: (bids: Bid[]) => void;
  addBid: (bid: Bid) => void;
}

export const useBidStore = createWithEqualityFn<State & Actions>((set) => ({
  bids: [],
  setBids: (bids: Bid[]) => {
    set(() => ({
      bids
    }))
  },
  addBid: (bid: Bid) => {
    set((state) => ({
      bids: !state.bids.find(b => b.id === bid.id) ? [bid, ...state.bids] : [...state.bids]
    }))
  }
}));