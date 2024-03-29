import {useParamsStore} from '@/hooks/useParamsStore';
import {Button} from 'flowbite-react';
import React from 'react'
import {AiOutlineClockCircle, AiOutlineSortAscending} from 'react-icons/ai';
import {BsFillStopCircleFill, BsStopwatchFill} from 'react-icons/bs';
import {GiFinishLine, GiFlame} from 'react-icons/gi';

const pageSizeButtons = [4, 8, 12];
const gridColumnsOptions = [2, 3, 4];

const orderButtons = [
  {
    label: '',
    icon: AiOutlineSortAscending,
    value: 'make'
  },
  {
    label: '',
    icon: AiOutlineClockCircle,
    value: 'endingSoon'
  },
  {
    label: '',
    icon: BsFillStopCircleFill,
    value: 'new'
  },
]

const filterButtons = [
  {
    label: 'Live',
    icon: GiFlame,
    value: 'live'
  },
  {
    label: 'Ending',
    icon: GiFinishLine,
    value: 'endingSoon'
  },
  {
    label: 'Done',
    icon: BsStopwatchFill,
    value: 'finished'
  },
]

export default function Filters() {
  const pageSize = useParamsStore(state => state.pageSize);
  const setParams = useParamsStore(state => state.setParams);
  const orderBy = useParamsStore(state => state.orderBy);
  const filterBy = useParamsStore(state => state.filterBy);
  const gridColumns = useParamsStore(state => state.gridColumns);

  console.log('Filters rendered', gridColumns)
  return (
      <div className='flex justify-between items-center mb-4'>

        <div>
          <span className='uppercase text-sm text-gray-500 mr-2'>Filter by</span>
          <Button.Group>
            {filterButtons.map(({label, icon: Icon, value}) => (
                <Button
                    key={value}
                    onClick={() => setParams({filterBy: value})}
                    color={`${filterBy === value ? 'red' : 'gray'}`}
                >
                  <Icon className='mr-3 h-4 w-4'/>
                  {label}
                </Button>
            ))}
          </Button.Group>
        </div>


        <div>
          <span className='uppercase text-sm text-gray-500 mr-2'>Order by</span>
          <Button.Group>
            {orderButtons.map(({label, icon: Icon, value}) => (
                <Button
                    key={value}
                    onClick={() => setParams({orderBy: value})}
                    color={`${orderBy === value ? 'red' : 'gray'}`}
                >
                  <Icon className='mr-3 h-4 w-4'/>
                  {label}
                </Button>
            ))}
          </Button.Group>
        </div>

        <div>
          <span className='uppercase text-sm text-gray-500 mr-2'>Grid size</span>
          <Button.Group>
            {gridColumnsOptions.map((value: number) => (
                <Button key={value}
                        onClick={() => setParams({gridColumns: value})}
                        color={`${gridColumns === value ? 'red' : 'gray'}`}
                >
                  {value}
                </Button>
            ))}
          </Button.Group>
        </div>

        <div>
          <span className='uppercase text-sm text-gray-500 mr-2'>Page size</span>
          <Button.Group>
            {pageSizeButtons.map((value, i) => (
                <Button key={i}
                        onClick={() => setParams({pageSize: value})}
                        color={`${pageSize === value ? 'red' : 'gray'}`}
                        className='focus:ring-0'
                >
                  {value}
                </Button>
            ))}
          </Button.Group>
        </div>
      </div>
  )
}
