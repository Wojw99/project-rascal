#ifndef __TS_QUEUE_H__
#define __TS_QUEUE_H__

#include "../Common/CommonIncludes.h"

template <typename T>
class Tsqueue
{
public:
	Tsqueue() = default;
	Tsqueue(const Tsqueue&) = delete;
	virtual ~Tsqueue() { this->clear(); };

protected:
	std::mutex muxQueue;
	std::deque <T> deqQueue;

public:
	const T& front()
	{
		std::scoped_lock lock(this->muxQueue);
		return this->deqQueue.front();
	}

	const T& back()
	{
		//protecting the mutex, only 1 thread have acces to this function
		std::scoped_lock lock(this->muxQueue);
		return this->deqQueue.back();
	}

	void push_back(const T& item)
	{
		std::scoped_lock lock(this->muxQueue);
		this->deqQueue.emplace_back(std::move(item));
	}

	void push_front(const T& item)
	{
		std::scoped_lock lock(this->muxQueue);
		this->deqQueue.emplace_front(std::move(item));
	}

	bool empty()
	{
		std::scoped_lock lock(this->muxQueue);
		return this->deqQueue.empty();
	}

	size_t count()
	{
		std::scoped_lock lock(this->muxQueue);
		return this->deqQueue.size();
	}

	void clear()
	{
		std::scoped_lock lock(this->muxQueue);
		this->deqQueue.clear();
	}

	T pop_front()
	{
		std::scoped_lock lock(this->muxQueue);
		// storing the object, which will be deleted
		auto t = std::move(this->deqQueue.front());
		this->deqQueue.pop_front();
		return t;
	}

	T pop_back()
	{
		std::scoped_lock lock(this->muxQueue);
		auto t = std::move(this->deqQueue.back());
		this->deqQueue.pop_back();
		return t;
	}

};

#endif