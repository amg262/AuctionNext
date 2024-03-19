import Heading from '@/app/components/Heading'
import React from 'react'
import {Payment} from "@/types";
import {getCurrentUser} from "@/app/actions/authActions";
import {completePayment} from "@/app/actions/auctionActions";

export default async function Update({params}: { params: { id: string } }) {
  // const user = await getCurrentUser();
  // const [payment, setPayment] = React.useState<Payment | null>();
  // // const [loading, setLoading] = React.useState(false);
  // const [error, setError] = React.useState(null);
  const user = await getCurrentUser();
  let payment: Payment | null = null;
  let error = null;
  // useEffect(() => {
  //   const validatePayment = async () => {
  //     setLoading(true);
  //     try {
  //       const response = await completePayment(params.id);
  //       setPayment(response);
  //       // const payment = await GetPayment(params.id);
  //       // setPayment(payment);
  //     } catch (error: any) {
  //       setError(error);
  //     } finally {
  //       setLoading(false);
  //     }
  //   };
  //   validatePayment().then(r => console.log(r)).catch(e => console.error(e));
  // }, [params.id]);

  // try {
  //   const response = await completePayment(params.id);
  //   setPayment(response);
  // } catch (err: any) {
  //   setError(err);
  // }
  try {
    payment = await completePayment(params.id);
  } catch (err: any) {
    error = err;
  }

  // if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='Payment Succcessful' subtitle='bahhhh'/>
        <h1>Helllllllo!</h1>

        <h3>{user?.username}</h3>

        <h3>{params.id}</h3>

        {payment && <pre>{JSON.stringify(payment, null, 2)}</pre>}

        {/*<Button onClick={async () => GetPayment(params.id)}>Refresh Payment Data</Button>*/}

        {/*<ValidatePaymentButton paymentId={params.id}/>*/}

        {/*{payment && <pre>{JSON.stringify(payment, null, 2)}</pre>}*/}

      </div>
  )
}
