'use client';

import {AiOutlineCar, AiOutlineTransaction} from "react-icons/ai";
import React from "react";
import {Payment, Shipping} from "@/types";

type Props = {
  payment?: Payment | null
  headers?: any
  user?: any
}

export default function CompleteShipping({payment, headers, user}: Props) {
  const [shipping, setShipping] = React.useState<Shipping | null>(null);
  const [isLoading, setIsLoading] = React.useState(false);
  const [error, setError] = React.useState(null);

  function ship() {
    setIsLoading(true);
    console.log('Shipping completed');

    const requestOptions = {
      method: 'POST',
      headers: headers,
    }

    console.log('user', user)
    console.log('headers', headers)

    try {
      const response = fetch(`https://api.auctionnext.com/shipping/complete/${payment?.id}`, requestOptions)
          .then(response => response.json())
          .then(data => {
            setShipping(data);
            setIsLoading(false);
          })
          .catch(error => {
            console.error(error);
            setError(error);
            setIsLoading(false);
          });
      // const response = fetchWrapper.post(`shipping/complete/${payment?.id}`, {});
      console.log(response);
    } catch (error) {
      console.error(error);
    }
  }

  const openTrackingUrl = () => {
    if (shipping?.trackingUrl) {
      window.open(shipping.trackingUrl, '_blank');
    }
  };


  return (
      <div>
        <button
            className={`flex items-center justify-center text-white font-bold py-2 px-4 rounded cursor-pointer ${isLoading ? 'bg-gray-500' : 'bg-blue-500 hover:bg-blue-700'}`}
            onClick={ship}
            disabled={isLoading}
        >
          <AiOutlineCar className="mr-2"/> {isLoading ? 'Processing...' : 'Complete Shipping'}
        </button>

        {shipping && (
            <button
                className="mt-4 flex items-center justify-center bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded cursor-pointer"
                onClick={openTrackingUrl}
            >
              <AiOutlineTransaction className="mr-2"/>
              Track Shipment
            </button>
        )}

        {error && (
            <div className="mt-4 p-4 bg-red-100 text-red-700 rounded-lg">
              {error}
            </div>
        )}

        {shipping && (
            <div className="mt-4 p-4 bg-gray-100 rounded-lg shadow">
              <h3 className="text-lg font-semibold">Shipping Details</h3>
              <div className="grid grid-cols-2 gap-4 mt-2">
                <p>Shipping ID: <span className="font-medium">{shipping.shippingId}</span></p>
                <p>Carrier: <span className="font-medium">{shipping.carrier}</span></p>
                <p>Tracking Code: <span className="font-medium">{shipping.trackingCode}</span></p>
                <p>View Tracking: <span className="font-medium">
                  <a rel="noopener noreferrer" className="text-blue-500 hover:text-blue-700 underline"
                     href={shipping.trackingUrl} target="_blank">
                    {shipping.trackingCode}
                  </a>
                </span></p>
                {/*<p>Tracking Url: <span className="font-medium">{shipping.trackingUrl}</span></p>*/}
                <p>Rate: <span className="font-medium">${shipping.rate}</span></p>
                <p>Name: <span className="font-medium">{shipping.name}</span></p>
                <p>Company: <span className="font-medium">{shipping.company}</span></p>
                <p>Address: <span
                    className="font-medium">{`${shipping.street1}, ${shipping.street2}, ${shipping.city}, ${shipping.state}, ${shipping.zip}, ${shipping.country}`}</span>
                </p>
                <p>Email: <span className="font-medium">{shipping.email}</span></p>
                <p>Last Updated: <span className="font-medium">{new Date(shipping.updatedAt).toLocaleString()}</span>
                </p>
              </div>
            </div>
        )}
      </div>
  );
}
