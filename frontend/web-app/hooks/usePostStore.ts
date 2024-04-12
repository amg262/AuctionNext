import {PagedResult, Post} from "@/types"
import {create} from "zustand"

type State = {
  posts: Post[]
  totalCount: number
  pageCount: number
}

type Actions = {
  setData: (data: PagedResult<Post>) => void
}

const initialState: State = {
  posts: [],
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
  setData: (data: PagedResult<Post>) => {
    set(() => ({
      posts: data.results,
      totalCount: data.totalCount,
      pageCount: data.pageCount
    }))
  },
}))