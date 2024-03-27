import React from 'react'

type Props = {
  title: string
  subtitle?: string
  center?: boolean
  children?: React.ReactNode
}

export default function Heading({title, subtitle, center, children}: Props) {
  return (
      <div className={center ? 'text-center' : 'text-start'}>
        <div className='flex items-center text-2xl font-bold'>
          {title}&nbsp;{children}
        </div>
        <div className='font-light text-neutral-500 mt-2'>
          {subtitle}
        </div>
      </div>
  )
}
